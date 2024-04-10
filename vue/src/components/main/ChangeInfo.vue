<template>.
  <div class="bigDiv">
    <el-form :model="form.data" label-width="auto" style="max-width: 360px">
      <!-- <el-form-item class="headPortrait" @click="headPortrait">
        <img :src="baseURL+form.data.headPortrait" alt="">
      </el-form-item> -->
      <el-form-item class="headPortrait">
        <el-upload class="avatar-uploader" action="http://localhost:5246/UploadFile/UploadFile" :show-file-list="false"
          :on-success="handleAvatarSuccess" :before-upload="beforeAvatarUpload">
          <img v-if="form.data.headPortrait" :src="baseURL+form.data.headPortrait" class="avatar" />
          <el-icon v-else class="avatar-uploader-icon">
            <Plus />
          </el-icon>
        </el-upload>
      </el-form-item>
      <el-form-item label="姓名">
        <el-input v-model="form.data.name" />
      </el-form-item>
      <el-form-item label="联系电话">
        <el-input v-model="form.data.phone" />
      </el-form-item>
      <el-form-item label="邮箱">
        <el-input v-model="form.data.mailbox" />
      </el-form-item>
      <el-form-item label="常住地">
        <el-input v-model="form.data.address" />
      </el-form-item>
      <el-form-item label="银行卡开户行">
        <el-input v-model="form.data.openingBank" />
      </el-form-item>
      <el-form-item label="银行卡号">
        <el-input v-model="form.data.cardNumber" />
      </el-form-item>
      <el-form-item label="身份证号">
        <el-input v-model="form.data.idNumber" />
      </el-form-item>
      <el-form-item class="headPortrait">
        <el-button type="primary" @click="UpdateButton">修改</el-button>
      </el-form-item>
    </el-form>
  </div>
</template>

<script lang="ts" setup>
import { reactive, onMounted, ref } from 'vue'

import { ElMessage } from 'element-plus'
import type { UploadProps } from 'element-plus'
import { baseURL, GetUserInfo as GetUserInfoApi, UpdateUserInfo as UpdateUserInfoAPI } from '@/ts/RequestInterface';
import Qs from 'qs'

const form = reactive({
  data: {
    name: '',
    headPortrait: '',
    phone: '',
    mailbox: '',
    address: '',
    openingBank: '',
    cardNumber: '',
    idNumber: '',
    id: 7,
    roleName: ''
  }

})

const handleAvatarSuccess: UploadProps['onSuccess'] = (
  response,
  uploadFile
) => {
  if(response){
    form.data.headPortrait = response;
    ElMessage({
      message:"头像上传成功，请保存",
      type: 'success',
      grouping: true,
    })
  }
  else{
    ElMessage.error('修改失败，请联系管理员')
  }
  //imageUrl.value = URL.createObjectURL(uploadFile.raw!)
  

}

const beforeAvatarUpload: UploadProps['beforeUpload'] = (rawFile) => {
  if (rawFile.type !== 'image/jpeg') {
    ElMessage.error('Avatar picture must be JPG format!')
    return false
  } else if (rawFile.size / 1024 / 1024 > 2) {
    ElMessage.error('Avatar picture size can not exceed 2MB!')
    return false
  }
  return true
}

const UpdateButton = (async () => {
  //console.log(form.data)
  const da = await UpdateUserInfoAPI(form.data)
  console.log(da);
  if(da.data){
    ElMessage({
      message:"修改成功",
      type: 'success',
      grouping: true,
    })
  }
  else{
    ElMessage.error('修改失败，请联系管理员')
  }
})

onMounted(async () => {
  //const datas = { userId: 7 };
  form.data = (await GetUserInfoApi()).data
  console.log(form.data);
});
</script>

<style scoped>
.bigDiv {
  margin: 0 34%;
}

.headPortrait {
  cursor: pointer;
  width: 100px;
  height: 100px;
  margin: 0 auto
}

.headPortrait img {
  max-width: 100px;
  max-height: 100px;
  border-radius: 50%;
}
</style>
