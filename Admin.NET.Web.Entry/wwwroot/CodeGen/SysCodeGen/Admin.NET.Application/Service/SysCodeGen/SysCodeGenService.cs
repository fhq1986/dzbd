using Admin.NET.Application.Const;
namespace Admin.NET.Application;
/// <summary>
/// 测试服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class SysCodeGenService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysCodeGen> _rep;
    public SysCodeGenService(SqlSugarRepository<SysCodeGen> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询测试
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<SysCodeGenOutput>> Page(SysCodeGenInput input)
    {
        var query= _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.AuthorName.Contains(input.SearchKey.Trim())
                || u.TablePrefix.Contains(input.SearchKey.Trim())
                || u.GenerateType.Contains(input.SearchKey.Trim())
                || u.ConfigId.Contains(input.SearchKey.Trim())
                || u.DbName.Contains(input.SearchKey.Trim())
                || u.DbType.Contains(input.SearchKey.Trim())
                || u.ConnectionString.Contains(input.SearchKey.Trim())
                || u.TableName.Contains(input.SearchKey.Trim())
                || u.NameSpace.Contains(input.SearchKey.Trim())
                || u.BusName.Contains(input.SearchKey.Trim())
                || u.CreateUserName.Contains(input.SearchKey.Trim())
                || u.UpdateUserName.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.AuthorName), u => u.AuthorName.Contains(input.AuthorName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TablePrefix), u => u.TablePrefix.Contains(input.TablePrefix.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.GenerateType), u => u.GenerateType.Contains(input.GenerateType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ConfigId), u => u.ConfigId.Contains(input.ConfigId.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.DbName), u => u.DbName.Contains(input.DbName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.DbType), u => u.DbType.Contains(input.DbType.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ConnectionString), u => u.ConnectionString.Contains(input.ConnectionString.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.TableName), u => u.TableName.Contains(input.TableName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.NameSpace), u => u.NameSpace.Contains(input.NameSpace.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.BusName), u => u.BusName.Contains(input.BusName.Trim()))
            .WhereIF(input.MenuPid>0, u => u.MenuPid == input.MenuPid)
            .WhereIF(!string.IsNullOrWhiteSpace(input.CreateUserName), u => u.CreateUserName.Contains(input.CreateUserName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.UpdateUserName), u => u.UpdateUserName.Contains(input.UpdateUserName.Trim()))
            .Select<SysCodeGenOutput>()
;
        query = query.OrderBuilder(input, "", "CreateTime");
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加测试
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddSysCodeGenInput input)
    {
        var entity = input.Adapt<SysCodeGen>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除测试
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteSysCodeGenInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.FakeDeleteAsync(entity);   //假删除
        //await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新测试
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateSysCodeGenInput input)
    {
        var entity = input.Adapt<SysCodeGen>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取测试
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<SysCodeGen> Get([FromQuery] QueryByIdSysCodeGenInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取测试列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<SysCodeGenOutput>> List([FromQuery] SysCodeGenInput input)
    {
        return await _rep.AsQueryable().Select<SysCodeGenOutput>().ToListAsync();
    }





}

