<template>
  <div class="sysCodeGen-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" labelWidth="90">
        <el-row>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10">
            <el-form-item label="关键字">
              <el-input v-model="queryParams.searchKey" clearable="" placeholder="请输入模糊查询关键字"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="AuthorName">
              <el-input v-model="queryParams.authorName" clearable="" placeholder="请输入AuthorName"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="TablePrefix">
              <el-input v-model="queryParams.tablePrefix" clearable="" placeholder="请输入TablePrefix"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="GenerateType">
              <el-input v-model="queryParams.generateType" clearable="" placeholder="请输入GenerateType"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="ConfigId">
              <el-input v-model="queryParams.configId" clearable="" placeholder="请输入ConfigId"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="DbName">
              <el-input v-model="queryParams.dbName" clearable="" placeholder="请输入DbName"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="DbType">
              <el-input v-model="queryParams.dbType" clearable="" placeholder="请输入DbType"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="ConnectionString">
              <el-input v-model="queryParams.connectionString" clearable="" placeholder="请输入ConnectionString"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="TableName">
              <el-input v-model="queryParams.tableName" clearable="" placeholder="请输入TableName"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="NameSpace">
              <el-input v-model="queryParams.nameSpace" clearable="" placeholder="请输入NameSpace"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="BusName">
              <el-input v-model="queryParams.busName" clearable="" placeholder="请输入BusName"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="MenuPid">
              <el-input v-model="queryParams.menuPid" clearable="" placeholder="请输入MenuPid"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="CreateUserName">
              <el-input v-model="queryParams.createUserName" clearable="" placeholder="请输入CreateUserName"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="4" class="mb10" v-if="showAdvanceQueryUI">
            <el-form-item label="UpdateUserName">
              <el-input v-model="queryParams.updateUserName" clearable="" placeholder="请输入UpdateUserName"/>
              
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="12" :lg="6" :xl="6" class="mb10">
            <el-form-item>
              <el-button-group>
                <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'sysCodeGen:page'"> 查询 </el-button>
                <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
                <el-button icon="ele-ZoomIn" @click="changeAdvanceQueryUI" v-if="!showAdvanceQueryUI"> 高级 </el-button>
                <el-button icon="ele-ZoomOut" @click="changeAdvanceQueryUI" v-if="showAdvanceQueryUI"> 隐藏 </el-button>
                
              </el-button-group>
              
              <el-button-group style="margin-left:20px">
                <el-button type="primary" icon="ele-Plus" @click="openAddSysCodeGen" v-auth="'sysCodeGen:add'"> 新增 </el-button>
                
              </el-button-group>
              
            </el-form-item>
            
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table
				:data="tableData"
				style="width: 100%"
				v-loading="loading"
				tooltip-effect="light"
				row-key="id"
				border="">
        <el-table-column type="index" label="序号" width="55" align="center"/>
        <el-table-column prop="authorName" label="AuthorName" width="150" show-overflow-tooltip="" />
        <el-table-column prop="tablePrefix" label="TablePrefix" width="165" show-overflow-tooltip="" />
        <el-table-column prop="generateType" label="GenerateType" width="180" show-overflow-tooltip="" />
        <el-table-column prop="configId" label="ConfigId" width="120" show-overflow-tooltip="" />
        <el-table-column prop="dbName" label="DbName" width="90" show-overflow-tooltip="" />
        <el-table-column prop="dbType" label="DbType" width="90" show-overflow-tooltip="" />
        <el-table-column prop="connectionString" label="ConnectionString" width="240" show-overflow-tooltip="" />
        <el-table-column prop="tableName" label="TableName" width="135" show-overflow-tooltip="" />
        <el-table-column prop="nameSpace" label="NameSpace" width="135" show-overflow-tooltip="" />
        <el-table-column prop="busName" label="BusName" width="105" show-overflow-tooltip="" />
        <el-table-column prop="menuPid" label="MenuPid" width="105" show-overflow-tooltip="" />
        <el-table-column prop="createUserName" label="CreateUserName" width="210" show-overflow-tooltip="" />
        <el-table-column prop="updateUserName" label="UpdateUserName" width="210" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="" v-if="auth('sysCodeGen:edit') || auth('sysCodeGen:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditSysCodeGen(scope.row)" v-auth="'sysCodeGen:edit'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delSysCodeGen(scope.row)" v-auth="'sysCodeGen:delete'"> 删除 </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination
				v-model:currentPage="tableParams.page"
				v-model:page-size="tableParams.pageSize"
				:total="tableParams.total"
				:page-sizes="[10, 20, 50, 100, 200, 500]"
				small=""
				background=""
				@size-change="handleSizeChange"
				@current-change="handleCurrentChange"
				layout="total, sizes, prev, pager, next, jumper"
	/>
      <editDialog
        ref="editDialogRef"
        :title="editSysCodeGenTitle"
        @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="sysCodeGen">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  import { getDictDataItem as di, getDictDataList as dl } from '/@/utils/dict-utils';
  //import { formatDate } from '/@/utils/formatTime';

  import editDialog from '/@/views/main/sysCodeGen/component/editDialog.vue'
  import { pageSysCodeGen, deleteSysCodeGen } from '/@/api/main/sysCodeGen';


  const showAdvanceQueryUI = ref(false);
  const editDialogRef = ref();
  const loading = ref(false);
  const tableData = ref<any>([]);
  const queryParams = ref<any>({});
  const tableParams = ref({
    page: 1,
    pageSize: 10,
    total: 0,
  });
  const editSysCodeGenTitle = ref("");

  // 改变高级查询的控件显示状态
  const changeAdvanceQueryUI = () => {
    showAdvanceQueryUI.value = !showAdvanceQueryUI.value;
  }
  

  // 查询操作
  const handleQuery = async () => {
    loading.value = true;
    var res = await pageSysCodeGen(Object.assign(queryParams.value, tableParams.value));
    tableData.value = res.data.result?.items ?? [];
    tableParams.value.total = res.data.result?.total;
    loading.value = false;
  };

  // 打开新增页面
  const openAddSysCodeGen = () => {
    editSysCodeGenTitle.value = '添加测试';
    editDialogRef.value.openDialog({});
  };

  // 打开编辑页面
  const openEditSysCodeGen = (row: any) => {
    editSysCodeGenTitle.value = '编辑测试';
    editDialogRef.value.openDialog(row);
  };

  // 删除
  const delSysCodeGen = (row: any) => {
    ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
  .then(async () => {
    await deleteSysCodeGen(row);
    handleQuery();
    ElMessage.success("删除成功");
  })
  .catch(() => {});
  };

  // 改变页面容量
  const handleSizeChange = (val: number) => {
    tableParams.value.pageSize = val;
    handleQuery();
  };

  // 改变页码序号
  const handleCurrentChange = (val: number) => {
    tableParams.value.page = val;
    handleQuery();
  };

  handleQuery();
</script>
<style scoped>
:deep(.el-ipnut),
:deep(.el-select),
:deep(.el-input-number) {
	width: 100%;
}
</style>

