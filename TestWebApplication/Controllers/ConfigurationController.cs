using Microsoft.AspNetCore.Mvc;
using TestWebApplication.Services;

namespace TestWebApplication.Controllers {
    public class ConfigurationController(IConfigurationService configurationService) : Controller {
        private readonly IConfigurationService _configurationService = configurationService;

        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadConfiguration(IFormFile configFile) {
            if (configFile == null) {
                ModelState.AddModelError("", "Please upload a valid JSON configuration file.");
                return View("Index");
            }

            var result = await _configurationService.ProcessConfigurationFileAsync(configFile);

            if (!result.IsSuccess) {
                ModelState.AddModelError("", result.ErrorMessage);
                return View("Index");
            }

            return RedirectToAction("DisplayConfiguration", new { id = result.ConfigurationRootId });
        }

        public async Task<IActionResult> DisplayConfiguration(int id) {
            var rootNode = await _configurationService.GetConfigurationTreeAsync(id);

            if (rootNode == null) {
                return NotFound();
            }

            return View(rootNode);
        }
    }
}
