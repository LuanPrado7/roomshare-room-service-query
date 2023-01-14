namespace roomshare_room_service_query.Model;

public class User
{
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string userType { get; set; } //PF & PJ
    public string? cpf { get; set; }
    public string? cnpj { get; set; }
    public string? corporateName { get; set; }
    public string? fantasyName { get; set; }
    public string? password { get; set; }
    public Guid guid { get; set; }
}
