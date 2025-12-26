namespace GreenConnectPlatform.Business.Models.Banks;

// Model trả về cho Mobile App
public class BankModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Tên đầy đủ (Ngân hàng TMCP Ngoại Thương...)
    public string Code { get; set; } = string.Empty; // Mã chữ (VCB)
    public string Bin { get; set; } = string.Empty; // Mã số (970436) - Dùng cái này để tạo QR
    public string ShortName { get; set; } = string.Empty; // Tên ngắn (Vietcombank)
    public string Logo { get; set; } = string.Empty; // URL Logo
}

// Model để hứng dữ liệu từ API VietQR (Cấu trúc phụ thuộc bên họ)
public class VietQrResponse<T>
{
    public string Code { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public T? Data { get; set; }
}