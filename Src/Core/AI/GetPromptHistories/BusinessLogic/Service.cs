using Base.DataBaseAndIdentity.Entities;
using Base.Gemini.Handler;
using FCommon.FeatureService;
using GetPromptHistories.Common;
using GetPromptHistories.DataAccess;
using GetPromptHistories.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace GetPromptHistories.BusinessLogic
{
    public sealed class Service : IServiceHandler<AppRequestModel, AppResponseModel>
    {
        private readonly Lazy<IRepository> _repository;
        private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

        public Service(
            Lazy<IRepository> repository,
            Lazy<IHttpContextAccessor> httpContextAccessor
        )
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AppResponseModel> ExecuteAsync(
            AppRequestModel request,
            CancellationToken cancellationToken
        )
        {
            // Kiểm tra tính hợp lệ của yêu cầu
            if (request.Page <= 0 || request.Size <= 0)
            {
                return new AppResponseModel
                {
                    AppCode = Constant.AppCode.VALIDATION_FAILED
                };
            }

            // Lấy thông tin người dùng từ JWT token
            var userId = _httpContextAccessor.Value.HttpContext.User.FindFirstValue("sub");
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new AppResponseModel
                {
                    AppCode = Constant.AppCode.UNAUTHORIZED
                };
            }

            // Lấy thông tin phân trang
            var page = request.Page;
            var size = request.Size;

            // Truy vấn các lịch sử của người dùng từ repository
            var histories = await _repository.Value.GetHistoriesByUserId(userId, page, size, cancellationToken);
            var totalCount = await _repository.Value.CountHistoriesByUserId(userId, cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalCount / size);

            // Lọc bỏ các trường không cần thiết (identityUser, messages) và sắp xếp theo createdAt giảm dần
            var filteredHistories = histories
                .Select(h => new HistoryModel()
                {
                    Action = h.Action,
                    PlantUMLCode = h.PlantUMLCode,
                    UserId = h.UserId,
                    Id = h.Id,
                    CreatedAt = h.CreatedAt,
                    UpdatedAt = h.UpdatedAt
                })
                .OrderByDescending(h => h.CreatedAt) // Sắp xếp theo createdAt giảm dần
                .ToList();

            // Trả về kết quả
            return new AppResponseModel
            {
                AppCode = Constant.AppCode.SUCCESS,
                Body = new AppResponseModel.BodyModel
                {
                    Histories = filteredHistories,
                    IsHasNextPage = page < totalPages,
                    IsHasPreviousPage = page > 1
                }
            };
        }
    }
}
