using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace NCD.Models {
    public class SearchViewModel {
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        public string Sex { get; set; }

        [Range(12, 100, ErrorMessage = "{0} must be between 12 and 100.")]
        public int? AgeFrom { get; set; }

        [Range(12, 100, ErrorMessage = "{0} must be between 12 and 100.")]
        [GreaterThan("AgeFrom", ErrorMessage = "'From age' must be less than 'To age' field.", PassOnNull = true)]
        public int? AgeTo { get; set; }

        [Range(50, 250, ErrorMessage = "{0} must be between 50 and 250.")]
        public double? HeightFrom { get; set; }

        [Range(50, 250, ErrorMessage = "{0} must be between 50 and 250.")]
        [GreaterThan("HeightFrom", ErrorMessage = "'From height' must be less than 'To height' field.", PassOnNull = true)]
        public double? HeightTo { get; set; }

        [Range(50, 250, ErrorMessage = "{0} must be between 50 and 250.")]
        public double? WeightFrom { get; set; }

        [Range(50, 250, ErrorMessage = "{0} must be between 50 and 250.")]
        [GreaterThan("WeightFrom", ErrorMessage = "'From weight' must be less than 'To weight' field.", PassOnNull = true)]
        public double? WeightTo { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(250, ErrorMessage = "The email is to long, it should not exceed 250 characters.")]
        public string Email { get; set; }

        public int? MaxNumberResults { get; set; }
    }
}