    namespace Hebi_Api.Features.Core.DataAccess.Models;

    public class Clinic : IBaseModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     To control subscription
        /// </summary>
        public bool IsActive { get; set; }

        #region Foreign keys
        public Guid? ClinicId { get; set; }
        public IEnumerable<User> Doctors { get; set; } = Enumerable.Empty<User>();
        public Guid? LastModifiedBy { get; set; }
        public Guid CreatedBy { get; set; }
        #endregion
    }
