namespace Check.Interfaces
{
    public interface ITimestamped
    {
        DateTime CreatedDate { get; set; }

        DateTime UpdatedDate { get; set; }
    }
}
