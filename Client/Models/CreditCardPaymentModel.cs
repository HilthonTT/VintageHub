using System.ComponentModel.DataAnnotations;

namespace VintageHub.Client.Models;

public class CreditCardPaymentModel
{
    [Required(ErrorMessage = "Credit card number is required")]
    [CreditCard(ErrorMessage = "Please enter a valid credit card number")]
    public string CreditCardNumber { get; set; }

    [Required(ErrorMessage = "Cardholder name is required")]
    public string CardholderName { get; set; }

    [Required(ErrorMessage = "Expiration month is required")]
    [Range(1, 12, ErrorMessage = "Please select a valid expiration month")]
    public int ExpirationMonth { get; set; }

    [Required(ErrorMessage = "Expiration year is required")]
    [Range(2023, 2100, ErrorMessage = "Please select a valid expiration year")]
    public int ExpirationYear { get; set; }

    [Required(ErrorMessage = "CVV code is required")]
    [RegularExpression("^[0-9]{3,4}$", ErrorMessage = "Please enter a valid CVV code")]
    public string CVV { get; set; }

    [Required(ErrorMessage = "Billing address is required")]
    public string BillingAddress { get; set; }

    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }

    [Required(ErrorMessage = "State is required")]
    public string State { get; set; }

    [Required(ErrorMessage = "Postal code is required")]
    [RegularExpression("^[0-9]{5}$", ErrorMessage = "Please enter a valid postal code")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Country is required")]
    public string Country { get; set; }

    [Required(ErrorMessage = "Payment amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid payment amount")]
    public decimal PaymentAmount { get; set; }
}
