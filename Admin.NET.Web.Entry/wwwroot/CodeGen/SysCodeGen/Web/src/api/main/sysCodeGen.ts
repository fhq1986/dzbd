import request from '/@/utils/request';
enum Api {
  AddSysCodeGen = '/api/sysCodeGen/add',
  DeleteSysCodeGen = '/api/sysCodeGen/delete',
  UpdateSysCodeGen = '/api/sysCodeGen/update',
  PageSysCodeGen = '/api/sysCodeGen/page',
}

// 增加测试
export const addSysCodeGen = (params?: any) =>
	request({
		url: Api.AddSysCodeGen,
		method: 'post',
		data: params,
	});

// 删除测试
export const deleteSysCodeGen = (params?: any) => 
	request({
			url: Api.DeleteSysCodeGen,
			method: 'post',
			data: params,
		});

// 编辑测试
export const updateSysCodeGen = (params?: any) => 
	request({
			url: Api.UpdateSysCodeGen,
			method: 'post',
			data: params,
		});

// 分页查询测试
export const pageSysCodeGen = (params?: any) => 
	request({
			url: Api.PageSysCodeGen,
			method: 'post',
			data: params,
		});


