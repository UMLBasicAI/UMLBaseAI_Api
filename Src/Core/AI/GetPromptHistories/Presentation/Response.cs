using Base.DataBaseAndIdentity.Entities;
using GetPromptHistories.Models;
using System.Text.Json.Serialization;

namespace GetPromptHistories.Presentation;

public sealed class Response
{
    [JsonIgnore]
    public int HttpCode { get; set; }  // Mã trạng thái HTTP

    public string AppCode { get; set; } = string.Empty;  // Mã ứng dụng (SUCCESS, ERROR,...)

    public BodyDto Body { get; set; }  // Dữ liệu trả về thực tế

    public sealed class BodyDto
    {
        public List<HistoryModel> Histories { get; set; }  // Danh sách thông điệp liên quan đến lịch sử

        public Boolean IsHasNextPage { get; set; }  // Có trang kế tiếp không

        public Boolean IsHasPreviousPage { get; set; }  // Có trang trước không
    }
}