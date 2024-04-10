<template>
  <div v-loading="loading">
    <el-table ref="tableRef" row-key="id" :data="filterTableData" style="width: 100%" :row-style="tableRowClassName">
      <el-table-column type="index" :index="indexMethod" />
      <el-table-column prop="name" label="Name" width="170" />
      <el-table-column prop="phone" label="Phone" width="170" />
      <el-table-column prop="roleName" label="RoleName" width="170" />
      <el-table-column prop="address" label="Address" width="170" />
      <el-table-column prop="lock" label="lock" :filters="[
    { text: false, value: false },
    { text: true, value: true },
  ]" :filter-method="filterLock" width="170" />

      <el-table-column fixed="right" width="210">
        <template #header>
          <el-input v-model="search" size="small" placeholder="Type to search" @keydown.enter="searchAction"
            style="float: left;width:100px" />
          <el-button type="info" plain size="small" style="margin-left:5px"
            @click="dialogFormVisible = true">添加</el-button>
        </template>

        <template #default="scope">
          <el-button size="small" @click="resetPasswords(scope.$index, scope.row)">重置密码</el-button>
          <el-button size="small" type="danger" @click="negationLock(scope.$index, scope.row)"
            v-if="!scope.row.lock">禁用</el-button>
          <el-button size="small" type="success" @click="negationLock(scope.$index, scope.row)" v-else>恢复</el-button>
        </template>
      </el-table-column>
    </el-table>
    <div class="el-pagination">
      <el-pagination layout="prev, pager, next" :total="totalNumber" @current-change="currentChange" />
    </div>

    <el-dialog v-model="dialogFormVisible" title="添加用户信息" width="500" draggable>
      <el-form ref="ruleFormRef" :model="form" :rules="rules">
        <el-form-item label="姓名" :label-width="formLabelWidth" prop="name">
          <el-input v-model="form.name" autocomplete="off" />
        </el-form-item>
        <el-form-item label="电话" :label-width="formLabelWidth" prop="phone">
          <el-input v-model="form.phone" autocomplete="off" />
        </el-form-item>
        <el-form-item label="角色" :label-width="formLabelWidth" prop="roleId">
          <el-select v-model="form.roleId" placeholder="请选择角色">
            <el-option label="管理员" value="1" />
            <el-option label="经理" value="21" />
            <el-option label="寄售审核" value="41" />
            <el-option label="合同审核" value="61" />
            <el-option label="普通用户" value="81" />
          </el-select>
        </el-form-item>
      </el-form>

      <template #footer>
        <div class="dialog-footer">
          <el-button @click="dialogFormVisible = false">取消</el-button>
          <el-button type="primary" @click="submitForm(ruleFormRef)">
            提交
          </el-button>
        </div>
      </template>
    </el-dialog>

  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, reactive, computed, watch } from 'vue'

import { ElMessage, FormRules, FormInstance } from 'element-plus'
import type { TableInstance } from 'element-plus'

import { GetUserInfo1 as GetUserInfoApi, ResetPasswords as ResetPasswordsApi, NegationLock as NegationLockApi, AddUser as AddUserApi } from '@/ts/RequestInterface'
const indexMethod = (index: number) => {
  return index + 1
}
const loading = ref(true)
interface User {
  name: string
  address: string
  phone: string
  roleName: string
  lock: boolean
  id: number
}

const tableRef = ref<TableInstance>();
const tableRowClassName = ({
  row,
  rowIndex,
}: {
  row: User
  rowIndex: number
}) => {
  if (row.lock) {
    return { "--el-table-tr-bg-color": " var(--el-color-warning-light-9)" }
  }
  return ''
}

const filterLock = (value: boolean, row: User) => {
  return row.lock === value
}
let tableData = reactive({
  newtable: [{
    name: 'Tom',
    address: 'No. 189, Grove St, Los Angeles',
    phone: '12343',
    roleName: '',
    lock: true,
    id: 0
  }]
}
)
/* 
let tableData: User[] = reactive([
  {
    name: 'Tom',
    address: 'No. 189, Grove St, Los Angeles',
    phone: '12343',
    roleName:'',
    lock: true,
    id: 0
  }
]) */
const filterTableData = computed(() =>
  tableData.newtable.filter(
    (data) =>
      !search.value || data.name.toLowerCase().includes(search.value.toLowerCase())
  )

)
const search = ref(' ')
const searchName = ref('')
const resetPasswords = async (index: number, row: User) => {
  const result = await ResetPasswordsApi(row.id);
  if (result.data) {
    ElMessage({
      message: '密码已重置',
      type: 'success',
    })
  }
  else {
    ElMessage.error('新密码与旧密码相同，重置失败');
  }
}

const negationLock = async (index: number, row: User) => {
  const result = await NegationLockApi(row.id);
  if (result.data) {
    ElMessage({
      message: '操作成功',
      type: 'success',
    })
    row.lock = !row.lock;
    //console.log(row.lock);

  }
  else {
    ElMessage.error('操作失败');
  }
}
const addUser = async () => {
  //dialogFormVisible.value = false
  // console.log(form);
  const result = await AddUserApi(form)
  if (result.data) {
    ElMessage({
      message: '添加成功',
      type: 'success',
    })
    searchAction();
  }
  else {
    ElMessage({
      message: result.msg,
      type: 'error',
    })
  }
}

const totalNumber = ref(0);
onMounted(async () => {
  var data = await GetUserInfoApi(1, "");
  tableData.newtable = data.data
  totalNumber.value = data.total;
  loading.value = false
  search.value = ""
})
const currentChange = async (pager: number) => {
  loading.value = true
  tableData.newtable = (await GetUserInfoApi(pager, searchName.value)).data
  loading.value = false
  search.value = "1"
  search.value = searchName.value
}
const searchAction = async () => {
  searchName.value = search.value;
  var dt = await GetUserInfoApi(1, searchName.value);
  tableData.newtable = dt.data
  totalNumber.value = dt.total;
  search.value = ""
  search.value = searchName.value
  //console.log(dt);
}


const dialogFormVisible = ref(false)
watch(dialogFormVisible, (oldValue, newValue) => {
  //console.log(oldValue,newValue);
  if (newValue) {
    form.name = ""
    form.phone = ""
    form.roleId = ""
  }
})
const formLabelWidth = '140px'
const form = reactive({
  name: '',
  phone: '',
  roleId: '',
})

const ruleFormRef = ref<FormInstance>()
const rules = reactive<FormRules<typeof form>>({
  name: [
    { required: true, message: '必填', trigger: 'blur' },
    { min: 2, max: 15, message: 'Length should be 2 to 15', trigger: 'blur' },
  ],
  phone: [
    { required: true, message: '必填', trigger: 'blur' },
    { min: 11, max: 11, message: 'Length should be 11', trigger: 'blur' },
  ],
  roleId: [
    {
      required: true, message: '必填', trigger: 'change',
    },
  ],
})
const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return
  await formEl.validate((valid, fields) => {
    if (valid) {
      addUser();
    } else {
      ElMessage({
        message: '请检查表单是否有未填写的内容',
        type: 'warning'
      })
      console.log('error submit!', fields)
    }
  })
}

</script>

<style scoped>
.el-pagination {
  position: fixed;
  bottom: 12%;
  left: 50%;
}

.el-table .warning-row {
  --el-table-tr-bg-color: var(--el-color-warning-light-9);
}

.el-table .success-row {
  --el-table-tr-bg-color: var(--el-color-success-light-9);
}
</style>