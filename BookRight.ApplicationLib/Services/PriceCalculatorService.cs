using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Discounts;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.FacadeLib.Commands.Pricing.DTOs;
using BookRight.FacadeLib.Commands.Pricing.Interfaces;
using BookRight.FacadeLib.DTO;

namespace BookRight.ApplicationLib.Services
{
    public class PriceCalculatorService : IPriceCalculatorService
    {
        private readonly IEnumerable<IDiscountStrategy> _strategies;
        private readonly IBookingRepository _bookingRepository;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly ICampaignRepository _campaignRepository;

        public PriceCalculatorService(
            IEnumerable<IDiscountStrategy> strategies,
            ICustomerRepository CustomerRepository,
            ITreatmentRepository treatmentRepository,
            IBookingRepository bookingRepository,
            ICampaignRepository campaignRepository)
        {
            ArgumentNullException.ThrowIfNull(strategies);
            ArgumentNullException.ThrowIfNull(CustomerRepository);
            ArgumentNullException.ThrowIfNull(treatmentRepository);
            ArgumentNullException.ThrowIfNull(bookingRepository);
            ArgumentNullException.ThrowIfNull(campaignRepository);


            _strategies = strategies;
            _CustomerRepository = CustomerRepository;
            _treatmentRepository = treatmentRepository;
            _bookingRepository = bookingRepository;
            _campaignRepository = campaignRepository;


            if (_strategies.Any() == false)
                throw new ArgumentException(
                    "There has to be atleast 1 strategy.", nameof(strategies));
        }


        async Task<PriceCalculatorDTO> IPriceCalculatorService.CalculatePrice(PriceCalculatorCommand command)
        {
            Lock _lock = new Lock();

            ArgumentNullException.ThrowIfNull(command);

            var Customer = await _CustomerRepository.GetByIdAsync(command.CustomerId)
            ?? throw new NotFoundException("Customer not found");

            var CustomerHistorySum = await _bookingRepository.GetBookingHistorySumAsync(command.CustomerId, 12);

            var treatment = await _treatmentRepository.GetByIdAsync(command.TreatmentId)
            ?? throw new NotFoundException($"Treatment with id {command.TreatmentId} not found");

            var allTreatments = await _treatmentRepository.GetAllAsync()
                ?? throw new NotFoundException("No treatments was found");

            var numberOfBirthdayDiscountUsed = await _bookingRepository.GetNumberOfBirthdayDiscountThisYearByIdAsync(command.CustomerId, command.BookingDate.Year);

            var activeCampaigns = await _campaignRepository.GetActiveCampaignsAsync()
                ?? throw new NotFoundException("No active campaigns found");

            var PriceCalculatorDTO = new PriceCalculatorInput(
                treatment.Price,
                command.BookingDate,
                Customer.BirthDate,
                CustomerHistorySum,
                numberOfBirthdayDiscountUsed,
                allTreatments,
                activeCampaigns);

            PriceCalculatorResult? bestDiscount = null;

            Parallel.ForEach(_strategies, s =>
                {
                    var discountResult = s.CalculatePrice(PriceCalculatorDTO);

                    lock (_lock)
                    {
                        if (
                            discountResult.IsApplicable == true &&
                            (
                                bestDiscount == null ||
                                discountResult.FinalPrice < bestDiscount.FinalPrice
                            )
                        )
                        {
                            bestDiscount = discountResult;
                        }

                    }
                });

            if (bestDiscount != null)
                return new PriceCalculatorDTO(bestDiscount.NormalPrice, bestDiscount.FinalPrice, bestDiscount.DiscountType.ToString());


            throw new Exceptions.ApplicationException("Error while calculating price");
        }
    }
}
