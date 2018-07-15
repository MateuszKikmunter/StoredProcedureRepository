Project with simple convention based repository for calling stored procedures and some extension methods to invoke stored procedures in a fluent way.
 
Extensions are working with standard and user-defined table type parameters, repository works only with standard parameters, so both can be used interchangeably depending on individual needs.
 
Project written because of a solution which I was working on which had very old database as source and I was able to get entities only with Linq to SQL or via stored procedures.
 
 Sample Usage:
 
 public IEnumerable<Client> GetActiveClientsByClientCode(string code, string userAdName, int menuFilter)
        {
            return _storedProcedureRepository.GetData<ClientSimple>("uspGetActiveClientsByClientCode",
                new { Code = code, UserAdName = userAdName, Filter = menuFilter })
                .GroupBy(c => new { c.Name, c.Number })
                .Select(c => c.First())
                .ToList();
        }