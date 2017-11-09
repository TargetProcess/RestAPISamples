namespace Tp.EntityConvertor
{
    /// <summary>
    /// Convertion general to entity type request dto.
    /// </summary>
    internal class ConvertGeneralToTypeDto
    {
        /// <summary>
        /// Entity type id to convert general with id <see cref="GeneralId"/> to.
        /// </summary>
        public int EntityTypeId { get; set; }

        /// <summary>
        /// General id to convert.
        /// </summary>
        public int GeneralId { get; set; }
    }
}