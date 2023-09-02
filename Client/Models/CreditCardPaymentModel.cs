namespace VintageHub.Client.Models;

public class CreditCardPaymentModel
{
    [Required(ErrorMessageResourceName = "credit_card_number_required", ErrorMessageResourceType = typeof(Resource))]
    [CreditCard(ErrorMessageResourceName = "credit_card_number_invalid", ErrorMessageResourceType = typeof(Resource))]
    public string CreditCardNumber { get; set; }

    [Required(ErrorMessageResourceName = "cardholder_name_required", ErrorMessageResourceType = typeof(Resource))]
    public string CardholderName { get; set; }

    [Required(ErrorMessageResourceName = "expiration_month_required", ErrorMessageResourceType = typeof(Resource))]
    [Range(1, 12, ErrorMessageResourceName = "expiration_month_invalid", ErrorMessageResourceType = typeof(Resource))]
    public int ExpirationMonth { get; set; }

    [Required(ErrorMessageResourceName = "expiration_year_required", ErrorMessageResourceType = typeof(Resource))]
    [Range(2023, 2100, ErrorMessageResourceName = "expiration_year_invalid", ErrorMessageResourceType = typeof(Resource))]
    public int ExpirationYear { get; set; }

    [Required(ErrorMessageResourceName = "cvv_code_required", ErrorMessageResourceType = typeof(Resource))]
    [RegularExpression("^[0-9]{3,4}$", ErrorMessageResourceName = "cvv_code_invalid", ErrorMessageResourceType = typeof(Resource))]
    public string CVV { get; set; }

    [Required(ErrorMessageResourceName = "billing_address_required", ErrorMessageResourceType = typeof(Resource))]
    public string BillingAddress { get; set; }

    [Required(ErrorMessageResourceName = "city_required", ErrorMessageResourceType = typeof(Resource))]
    public string City { get; set; }

    [Required(ErrorMessageResourceName = "state_required", ErrorMessageResourceType = typeof(Resource))]
    public string State { get; set; }

    [Required(ErrorMessageResourceName = "postal_code_required", ErrorMessageResourceType = typeof(Resource))]
    [RegularExpression("^[0-9]{5}$", ErrorMessageResourceName = "postal_code_invalid", ErrorMessageResourceType = typeof(Resource))]
    public string PostalCode { get; set; }

    [Required(ErrorMessageResourceName = "country_required", ErrorMessageResourceType = typeof(Resource))]
    public string Country { get; set; }

    [Required(ErrorMessageResourceName = "payment_amount_required", ErrorMessageResourceType = typeof(Resource))]
    [Range(0.01, double.MaxValue, ErrorMessageResourceName = "payment_amount_invalid", ErrorMessageResourceType = typeof(Resource))]
    public decimal PaymentAmount { get; set; }
}
