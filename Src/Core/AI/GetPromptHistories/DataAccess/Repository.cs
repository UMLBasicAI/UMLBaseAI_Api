using Base.DataBaseAndIdentity.DBContext;
using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GetPromptHistories.DataAccess
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _dbContext;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HistoryEntity>> GetHistoriesByUserId(string userId, int page, int size, CancellationToken cancellationToken)
        {
            var userGuid = Guid.Parse(userId);

            // Truy vấn các lịch sử của người dùng, có phân trang
            return await _dbContext.Set<HistoryEntity>()
                .Where(h => h.UserId == userGuid)
                .OrderByDescending(h => h.CreatedAt) // Sắp xếp theo thời gian tạo
                .Skip((page - 1) * size) // Bỏ qua các mục trước đó theo trang
                .Take(size) // Lấy số lượng theo kích thước trang
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountHistoriesByUserId(string userId, CancellationToken cancellationToken)
        {
            var userGuid = Guid.Parse(userId);

            // Đếm tổng số lịch sử của người dùng
            return await _dbContext.Set<HistoryEntity>()
                .Where(h => h.UserId == userGuid)
                .CountAsync(cancellationToken);
        }
    }
}
