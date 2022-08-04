using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly()); // Lấy tất cả những thằng đang thực thi
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var mapFromType = typeof(IMapFrom<>);

            const string mappingMethodName = nameof(IMapFrom<object>.Mapping); // Khai báo cái MapFrom

            bool HasInterface(Type t) => t.IsGenericType // Khai báo 1 cái tên phương thức và kiểm tra xem có cái interface nào mà đúng theo cái interface mapFromType đó không
                                         && t.GetGenericTypeDefinition() == mapFromType;

            var types = assembly.GetExportedTypes() // Nếu có thì sẽ export tât cả các list đó ra => Thành 1 danh sách (Tất cả các Dto nào có triển khai)
            .Where(t => t.GetInterfaces()
                .Any(HasInterface)).ToList();

            var argumentTypes = new Type[] { typeof(Profile) };

            foreach (var type in types) // Duyệt qua tất cả
            {
                var instance = Activator.CreateInstance(type); // Khởi tạo 1 cái instance, instance này chính là của Dto

                var methodInfo = type.GetMethod(mappingMethodName); // Lấy method Info

                if (methodInfo != null) // Nếu không có thì khởi tạo
                {
                    methodInfo.Invoke(instance, new object[] { this }); // Invoke, khởi tạo cái instance đó lên
                }
                else
                {
                    var interfaces = type.GetInterfaces() // Lấy các method name nào mà sử dụng đúng cái interface mà chúng ta đã định nghĩa (IMapFrom)
                        .Where(HasInterface).ToList();

                    if (interfaces.Count <= 0) continue;

                    foreach (var @interface in interfaces) // Lọc qua hết 1 vòng
                    {
                        var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes); // Lấy ra toàn bộ thông tin

                        interfaceMethodInfo?.Invoke(instance, new object[] { this }); // Invoke cái Dto đó lên và copy tất cả các giá trị từ Entity qua hoặc từ Dto qua Entity // 9:45
                    }
                }
            }
        }
    }
}
