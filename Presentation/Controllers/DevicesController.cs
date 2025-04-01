using Domain.Entities;
using Domain.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/devices")]
    [Produces("application/json")]
    public class DevicesController : ControllerBase {
        private readonly IDeviceRepository deviceRepository;
        private readonly IUnitOfWork unitOfWork;

        public DevicesController(IDeviceRepository deviceRepository, IUnitOfWork unitOfWork) {
            this.deviceRepository = deviceRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Returns a list of devices
        /// </summary>
        /// <returns>List of all devices or an error message</returns>
        /// <response code="400"> The request was successful but there is no content</response>
        /// <response code="200"> The request was successful. Returns a list of all devices</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get() {
            var devices = await deviceRepository.GetAllDevicesAsync();

            if (devices == null || !devices.Any()) {
                return StatusCode(400, "There is no content");
            }

            return StatusCode(200, devices);
        }

        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="type">Device type</param>
        /// <param name="id">Device id</param>
        /// <returns>Success or eerror message</returns>
        /// <response code="400">The request was failure. The device has not been created and an error message is returned</response>
        /// <response code="201">The request was successful. The device has been created and a success message is retunred</response>
        [HttpPost("{id}")]
        public async Task<IActionResult> Post([FromBody]string type, int id =-1) {
            var isIdUnique = await deviceRepository.GetByIdAsync(id) == null;

            var deviceForSave = Device.Create(id, type, isIdUnique);

            if (deviceForSave.IsFailure) {
                return StatusCode(400, deviceForSave.Error.Message);
            }

            await deviceRepository.CreateAsync(deviceForSave.Value());

            await unitOfWork.SaveChangesAsync();

            return StatusCode(201, "The device has been created");
        }

        /// <summary>
        /// Delete device by id
        /// </summary>
        /// <param name="id">Device id</param>
        /// <returns>A success or eerror message</returns>
        /// <response code="400">The request was failure. The device has not been delted and a an error message is returned</response>
        /// <response code="200">The request was successful. The device has been deleted and a success message is returned</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var isDeleted = await deviceRepository.DeleteByIdAsync(id);

            if (!isDeleted) {
                return StatusCode(400, "There is no device to delete");
            }

            return StatusCode(200, "The device has been deleted");
        }

        /// <summary>
        /// Delete devices by type
        /// </summary>
        /// <param name="type">Device type</param>
        /// <returns>A success or eerror message</returns>
        /// <response code="400">The request was failure. The devices has not been delted and a an error message is returned</response>
        /// <response code="200">The request was successful. The devices has been deleted and a success message is returned</response>
        [HttpDelete("type/{type}")]
        public async Task<IActionResult> Delete(string type) {
            var isDeleted = await deviceRepository.DeleteByTypeAsync(type);

            if (!isDeleted) {
                return StatusCode(400, "There is no device to delete");
            }

            return StatusCode(200, "The devices has been deleted");
        }
    }
}
