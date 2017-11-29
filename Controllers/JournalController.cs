using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

using Zarasa.Editorial.Api.Models;
using Zarasa.Editorial.Api.Data;
using Zarasa.Editorial.Api.Repository;
using Zarasa.Editorial.Api.Request;
using Zarasa.Editorial.Api.Helper;
using System.Globalization;

namespace Zarasa.Editorial.Api.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public class JournalController : EntityController<Journal>
    {
        private readonly JournalRepository _repository;
        private readonly IEmailSender _emailSender;
        private readonly JWTSettings _options;
        private readonly ClientSettings _clientSettings;

        public JournalController(ApplicationDbContext context, 
            IEmailSender emailSender,
            IOptions<JWTSettings> optionsAccessor,
            IOptions<ClientSettings> clientSettingsAccessor) : base(context)
        {
            _repository = new JournalRepository(context);
            _emailSender = emailSender;
            _options = optionsAccessor.Value;
            _clientSettings = clientSettingsAccessor.Value;
        }

        protected override EntityRepository<Journal> GetRepository() => _repository;

        [HttpGet("get-all")]
        public override IActionResult GetAll(int? page, int? size) { 
            long count;
            var data = _repository.Get(page, ref size, out count);
            return EntityListResponse(data, pageNumber:page==null?0:page.Value, pageSize:size==null?0:size.Value, totalRecords:count);
        }

        [HttpGet("get-pending")]
        public IActionResult GetAllPending(string name, int? page, int? size) { 
            long count;
            var data = _repository.GetByName(name, Journal.JournalStatus.Panding, page, ref size, out count);
            return EntityListResponse(data, pageNumber:page==null?0:page.Value, pageSize:size==null?0:size.Value, totalRecords:count);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllActive(string name, int? page, int? size) { 
            long count;
            var data = _repository.GetByName(name, Journal.JournalStatus.Active, page, ref size, out count);
            return EntityListResponse(data, pageNumber:page==null?0:page.Value, pageSize:size==null?0:size.Value, totalRecords:count);
        }

        [AllowAnonymous]
        [HttpPost("request-journal")]
        public IActionResult RequestJournal([FromBody] JournalRequest journalRequest) 
        {
            IActionResult result = null;
            try{
                if (journalRequest == null)
                {
                    result = BadRequest();
                }
                if (result==null && !ModelState.IsValid)
                {
                    result = ValidationFailed();
                }
                if (result==null)
                {
                    var newEntity = journalRequest.toJournal();
                    var entity = GetRepository().Create(newEntity);
                    result = EntityResponse(entity, "Your Journal request created. You will get an email from admin when your request accepted or rejected");
                }
            } catch(Exception e){
                result = ErrorResponse(e.Message);
            }
            return result;
        }

        [HttpGet("{id}")]
        public override IActionResult GetById(long id) => base.GetById(id);

        [HttpPost]
        public override IActionResult Create([FromBody] Journal entity) => base.Create(entity);
        

        [HttpPut("{id}")]
        public override IActionResult Update(long id, [FromBody] Journal updatedEntity) => base.Update(id, updatedEntity);
        
       
        [HttpDelete("{id}")]
        public override IActionResult Delete(long id) => base.Delete(id);

        [HttpPut("activate/{id}")]
        public IActionResult Activate(long id) {
            IActionResult result = null;
            try{
                var journal = _repository.Activate(id, long.Parse(GetCurrentUserId()));
                if(!String.IsNullOrEmpty(journal.admin_email)) {
                    var jwt = new JWTHelper(_options); 
                    string confirmationToken = jwt.GetAccessToken(journal.admin_email);
                    string url = _clientSettings.BaseUrl + "/" + journal.name.ToSnakeCase()
                        + "/register?id=" + journal.id + "&token=" + confirmationToken;
                    SendActivationEmail(url);
                }
                result = MessageResponse("Journal Activated Successfully.");
            } catch (Exception e){
                result = ErrorResponse(e.Message);
            }
            return  result;
        } 

        private async Task SendActivationEmail(string url)
        {
           await _emailSender.SendEmailAsync("m_zainulabedin@yahoo.com", "Journal Activated",
                        $"Your journal has been activated. <br/><br/> Please click on the link blow"
                        + $" to proceed <br/> <a href='" + url + "'>" + url + "<a>");
        }
        
    }


}