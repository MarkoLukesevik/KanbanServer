namespace KanbanApp.Responses
{
    public class MinifiedBoardResponse
    {
        public MinifiedBoardResponse(Guid id, String name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
