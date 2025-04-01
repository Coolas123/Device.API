using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities
{
    public sealed class Device : Entity
    {
        public string Type { get; private set; }

        private Device(int id, string type) : base(id) {
            Type = type;
        }

        public static Result<Device> Create(int id, string type, bool isDeviceExist) {
            if (!isDeviceExist) {
                return Result.Failure<Device>(new Error("Device.Create", "the id is not unique"));
            }

            if (int.IsNegative(id)) {
                return Result.Failure<Device>(new Error("Device.Create","the id is negative"));
            }

            if (string.IsNullOrEmpty(type)) {
                return Result.Failure<Device>(new Error("Device.Create", "the type is not defined"));
            }

            return Result.Success(new Device(id, type));
        }
    }
}
