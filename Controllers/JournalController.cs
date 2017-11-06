using Microsoft.AspNetCore.Mvc;

using Zarasa.Editorial.Api.Models;
using Zarasa.Editorial.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Zarasa.Editorial.Api.Repository;

namespace Zarasa.Editorial.Api.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public class JournalController : EntityController<Journal>
    {
        private readonly JournalRepository _repository;
        public JournalController(ApplicationDbContext context) : base(context)
        {
            _repository = new JournalRepository(context);
        }

        protected override EntityRepository<Journal> GetRepository() => _repository;

        [HttpGet]
        public IActionResult GetAll(string name, int? page, int? size) { 
            long count;
            var data = _repository.GetByName(name, page, ref size, out count);
            return EntityListResponse(data, pageNumber:page==null?0:page.Value, pageSize:size==null?0:size.Value, totalRecords:count);
        }

        [HttpGet("{id}")]
        public override IActionResult GetById(long id) => base.GetById(id);

        [HttpPost]
        public override IActionResult Create([FromBody] Journal entity) => base.Create(entity);
        

        [HttpPut("{id}")]
        public override IActionResult Update(long id, [FromBody] Journal updatedEntity) => base.Update(id, updatedEntity);
        
       
        [HttpDelete("{id}")]
        public override IActionResult Delete(long id) => base.Delete(id);

    }


}