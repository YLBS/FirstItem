<template>
  <el-table :data="tableData" style="width: 100%" v-if="!loading" v-loading="loading">
    <el-table-column type="index" :index="indexMethod" />
    <el-table-column label="Date" width="180">
      <template #default="scope">
        <div style="display: flex; align-items: center">
          <el-icon>
            <timer />
          </el-icon>
          <span style="margin-left: 10px">{{ scope.row.date }}</span>
        </div>
      </template>
    </el-table-column>

    <el-table-column label="CarId" width="180">
      <template #default="scope">
        <el-popover effect="light" trigger="hover" placement="top" width="auto">
          <template #default>
            <div>CarId: {{ scope.row.CarId }}</div>
          </template>
          <template #reference>
            <el-tag>{{ scope.row.CarId }}</el-tag>
          </template>
        </el-popover>
      </template>
    </el-table-column>

    <el-table-column label="Examine" width="180">
      <template #default="scope">

        <el-button size="small" @click="handCheck(0,scope.row.CarId)">查看</el-button>
      </template>
    </el-table-column>

    <el-table-column label="State">
      <template #default="scope">
        <el-button size="small" type="danger" @click="handCheck(scope.row.Id, scope.row.CarId)"
          v-if="scope.row.state == '待审核'">{{
            scope.row.state }}</el-button>
        <el-tag v-if="scope.row.state != '待审核'">{{ scope.row.state }}</el-tag>
      </template>
    </el-table-column>

  </el-table>
  <el-drawer v-model="drawer" title="汽车详细信息" :direction="direction" :before-close="handleClose" id="Test">

    <el-form ref="ruleFormRef" :model="inputCar" label-width="110px" :inline="true" status-icon>

      <el-form-item label="车主" prop="Proposer"  class="w-50">
        <el-input v-model="inputCar.proposer" disabled />
      </el-form-item>

      <el-form-item label="品牌名称" prop="BrandName"  class="w-50">
        <el-input v-model="inputCar.brandName" disabled />
      </el-form-item>
      <el-form-item label="车型" prop="Name"  class="w-50">
        <el-input v-model="inputCar.name" disabled />
      </el-form-item>
      <el-form-item label="车源地" prop="VehicleSource"  class="w-50">
        <el-input v-model="inputCar.vehicleSource" disabled />
      </el-form-item>

      <el-form-item label="车身颜色" prop="VehicleColor"  class="w-50">
        <el-input v-model="inputCar.vehicleColor" disabled />
      </el-form-item>
      <el-form-item label="过户次数" prop="TransfersNumber"  class="w-50">
        <el-input v-model="inputCar.transfersNumber" type="text" autocomplete="off" disabled />
      </el-form-item>
      <el-form-item label="变速箱类型" prop="TransmissionType"  class="w-50">
        <el-input v-model="inputCar.transmissionType" disabled />
      </el-form-item>
      <el-form-item label="燃油类型" prop="Fuel"  class="w-50">
        <el-select v-model="inputCar.fuel" disabled>
          <el-option label="汽油" value="汽油" />
        </el-select>
      </el-form-item>
      <el-form-item label="座位数" prop="Seating"  class="w-50">
        <el-select v-model="inputCar.seating" disabled>
          <el-option label="1" value="1" />
        </el-select>
      </el-form-item>
      <el-form-item label="上牌时间"  class="w-50">
        <el-input v-model="inputCar.registrationTime" disabled />
      </el-form-item>


      <el-form-item label="车龄" prop="vehicleAge"  class="w-50">
        <el-input v-model.number="inputCar.vehicleAge" type="text" autocomplete="off" disabled />
      </el-form-item>

      <el-form-item label="公里数" prop="Mileage"  class="w-50">
        <el-input v-model="inputCar.mileage" type="text" autocomplete="off" style="width: 150px;" disabled />
        <span style="margin-left: 8px;"> 单位/万</span>
      </el-form-item>
      <el-form-item label="类型" prop="vehicleLevelName" required class="w-50" v-if="edit">
        <el-select v-model="vehicleLevelName" multiple collapse-tags collapse-tags-tooltip on-change="">
          <el-option label="SUV" value="SUV" :key="1"/>
          <el-option label="MPV" value="MPV" :key="2"/>
          <el-option label="轿车" value="轿车" :key="3"/>
          <el-option label="货车" value="货车" :key="4"/>
          <el-option label="皮卡" value="皮卡" :key="5"/>
          <el-option label="轻客" value="轻客" :key="6"/>
        </el-select>
      </el-form-item>
      <el-form-item label="类型" prop="vehicleLevelName"  class="w-50" v-else>
        <el-select v-model="vehicleLevelName" disabled>
        </el-select>
      </el-form-item>
      <el-form-item label="售价" prop="SellingPrice"  class="w-50">
        <el-input v-model="inputCar.sellingPrice" disabled style="width: 150px;" />
        <span style="margin-left: 8px;"> 单位/万</span>
      </el-form-item>

      <el-form-item label="是否支持分期" prop="Amortize"  class="w-50">
        <el-switch v-model="inputCar.amortize" disabled />
      </el-form-item>
      <el-form-item label="首付" prop="DownPayment"  class="w-50">
        <el-input v-model="inputCar.downPayment" disabled style="width: 150px;" />
        <span style="margin-left: 8px;"> 单位/万</span>
      </el-form-item>


      <el-form-item label="行驶证" prop="Registration" required class="w-50">

        <el-upload v-model:file-list="fileList" list-type="picture-card" :on-preview="handlePictureCardPreview"
          :disabled="true">
        </el-upload>

        <el-dialog v-model="dialogVisible">
          <img w-full :src="dialogImageUrl" alt="Preview Image" width="400px" />
        </el-dialog>
      </el-form-item>

      <el-form-item label="外观" prop="Appearance" required class="w-50">

        <el-upload v-model:file-list="fileList1" list-type="picture-card" :on-preview="handlePictureCardPreview"
          :disabled="true">
        </el-upload>

      </el-form-item>

      <el-form-item label="内饰" prop="VehiTrim" required class="w-50">

        <el-upload v-model:file-list="fileList2" list-type="picture-card" :on-preview="handlePictureCardPreview"
          :disabled="true">
        </el-upload>

      </el-form-item>

      <el-form-item label="车况" prop="VehicleCondition" style="width: 90%;">
        <el-input v-model="inputCar.vehicleCondition" type="textarea" :autosize="{ minRows: 4, maxRows: 6 }" disabled />
      </el-form-item>
      <el-form-item v-if="edit">
        <el-button type="primary" @click="Agree">
          同意
        </el-button>
        <el-button @click="Disagree">不同意</el-button>
      </el-form-item>

    </el-form>


    <el-dialog v-model="visible" :show-close="false">
      <template #header="{ close, titleId, titleClass }">
        <div class="my-header">
          <h4 :id="titleId" :class="titleClass">请填写不同意的原因</h4>
          <el-button type="danger" @click="close">
            <el-icon class="el-icon--left">
              <CircleCloseFilled />
            </el-icon>
            Close
          </el-button>
        </div>
      </template>
      <el-input v-model="textarea2" :autosize="{ minRows: 5, maxRows: 20 }" type="textarea" placeholder="Please input" />
      <el-button type="primary" style="margin-top: 20px;" @click="confirmDisagree">确定</el-button>
    </el-dialog>

  </el-drawer>
  <div class="el-pagination">
    <el-pagination layout="prev, pager, next"  v-model:page-size="pageSize4" :total="totalNumber" @current-change="currentChange" />
  </div>
  </template>

<script lang="ts" setup>
import { Timer } from '@element-plus/icons-vue'
import { ref, onMounted, reactive } from 'vue'
import { ElMessageBox, ElMessage, UploadProps } from 'element-plus'
import Qs from 'qs'

import {baseURL, GeConOrder as GeConOrderApi, GetCarInfo as GetCarInfoApi, UpdateState as UpdateStatesApi } from '@/ts/RequestInterface'
interface User {
  Id: number,
  date: string
  CarId: string
  state: string
  condition: string
}
interface FileData {
  name: string
  url: string
}
const pageSize4=ref(12)
const  vehicleLevelName=ref('')//汽车类型

let inputCar = reactive({
  vehicleAge: '',
  brandName: '',
  name: '',
  transmissionType: '',
  mileage: '',
  seating: '',
  vehicleLevelName:'',
  fuel: '',
  registrationTime: '',//上牌日期
  vehicleSource: '',//车源地
  vehicleColor: '',
  transfersNumber: '',//过户次数
  registration: '',//行驶证
  appearance: '',//外观
  vehiTrim: '',
  vehicleCondition: '',//描述
  sellingPrice: '',//售价
  amortize: false,//是否分期
  downPayment: '',//首付
  proposer: ''
})

const drawer = ref(false)
const visible = ref(false)

const direction = ref('rtl')
const handleClose = (done: () => void) => {
  done()
}
const loading = ref(true)
const textarea2 = ref('')
const totalNumber=ref(0)
const currentPage=ref(1);

let fileList = reactive<[FileData]>([
  {
    name: 'food.jpeg',
    url: '@/assets/1.jpg',
  }
])
let fileList1 = reactive<[FileData]>([
  {
    name: 'food.jpeg',
    url: '@/assets/1.jpg',
  }
])
let fileList2 = reactive<[FileData]>([
  {
    name: 'food.jpeg',
    url: '@/assets/1.jpg',
  }
])
const dialogImageUrl = ref('')
const dialogVisible = ref(false)


const handlePictureCardPreview: UploadProps['onPreview'] = (uploadFile) => {
  dialogImageUrl.value = uploadFile.url!
  dialogVisible.value = true
}


let conId = ref(0);
let edit=ref(true);


const handCheck = async (Id: number, CId: number) => {
  //alert(Id)
  if(Id==0){
    edit.value=false
  }
  else{
    edit.value=true
  }
  fileList.splice(0);
  fileList1.splice(0);
  fileList2.splice(0);
  conId.value = Id;
  const data = { Id: CId }
  inputCar = (await GetCarInfoApi(Qs.stringify(data))).data;
  
  vehicleLevelName.value=inputCar.vehicleLevelName;
  
  if (inputCar.registration != null) {
    var AVR = inputCar.registration.split(";")
    for (let i = 0; i < AVR.length; i++) {
      //console.log(AVR[i]);
      if (AVR[i] != '') {
        fileList[i] = {
          name: "123",
          url: baseURL + AVR[i]
        }
      }
    }
  }
  if (inputCar.appearance != null) {
    var AVR = inputCar.appearance.split(";")
    for (let i = 0; i < AVR.length; i++) {
      //console.log(AVR[i]);
      if (AVR[i] != '') {
        fileList1[i] = {
          name: "123",
          url: baseURL + AVR[i]
        }
      }
    }
  }

  if (inputCar.vehiTrim != null) {
    var AVR = inputCar.vehiTrim.split(";")
    for (let i = 0; i < AVR.length; i++) {
      //console.log(AVR[i]);
      if (AVR[i] != '') {
        fileList2[i] = {
          name: "123",
          url: baseURL + AVR[i]
        }
      }
    }
  }
  drawer.value = true
}
const Disagree = () => {
  textarea2.value = '';
  visible.value = true;
}
const confirmDisagree = async () => {
  //visible.value = true;
  if (textarea2.value == '') {
    ElMessage.error('请填写不同意的原因');
  }
  else {  //调用接口提交
    const data1 = { Id: conId.value, Idea: "不同意", Option: textarea2.value };
    const result = await UpdateStatesApi(Qs.stringify(data1));
    if (result.data) {
      ElMessage({
        message: '修改成功',
        type: 'success',
      })
      loading.value=true;
      var datas = await GeConOrderApi(currentPage.value);
      for (var i = 0; i < datas.data.length; i++) {
        tableData[i] = {
          Id: datas.data[i].id,
          date: datas.data[i].createTime,
          CarId: datas.data[i].carInfoId,
          state: datas.data[i].state,
          condition: datas.data[i].condition
        }
      }
      loading.value=false;
    }
    else {
      ElMessage.error('出错了');
    }
    drawer.value = false;
    visible.value = false;
  }
}

const Agree = async () => {  
  if (vehicleLevelName.value == '') {
    ElMessage.error('请补充汽车类型');
  }
  else {
    let vn=ref('');
    for(let i=0;i<vehicleLevelName.value.length;i++){
      vn.value +=vehicleLevelName.value[i]+";"
    }
    const data1 = { Id: conId.value, Idea: "同意", Option: vn.value };
    const result = await UpdateStatesApi(Qs.stringify(data1));
    if (result.data) {
      ElMessage({
        message: '修改成功',
        type: 'success',
      })
      loading.value=true;
      var datas = await GeConOrderApi(currentPage.value);
      for (var i = 0; i < datas.data.length; i++) {
        tableData[i] = {
          Id: datas.data[i].id,
          date: datas.data[i].createTime,
          CarId: datas.data[i].carInfoId,
          state: datas.data[i].state,
          condition: datas.data[i].condition
        }
      }
      loading.value=false;
      drawer.value = false;
      visible.value = false;
    }
    else {
      ElMessage.error('出错了');
    }
  }
}
const tableData:  User[] = reactive([]) 

tableData[0] = {
  Id: 1,
  date: "2024-01-08",
  CarId: "11",
  state: "已通过审核",
  condition: ""
}

onMounted(async () => {
  var datas = await GeConOrderApi(currentPage.value);
  for (var i = 0; i < datas.data.length; i++) {
    tableData[i] = {
      Id: datas.data[i].id,
      date: datas.data[i].createTime,
      CarId: datas.data[i].carInfoId,
      state: datas.data[i].state,
      condition: datas.data[i].condition
    }
  }
  totalNumber.value=datas.tatleCount;
  console.log("供",totalNumber.value);
  
  loading.value = false

})
const currentChange=async (pager:number)=>{
  
  var datas =  (await GeConOrderApi(pager)).data;
  tableData.length=0;
  for (var i = 0; i < datas.length; i++) {
    tableData[i] = {
      Id: datas[i].id,
      date: datas[i].createTime,
      CarId: datas[i].carInfoId,
      state: datas[i].state,
      condition: datas[i].condition
    }
  }
  currentPage.value=pager;
  
}
const indexMethod = (index: number) => {
  return index + 1
}
</script >
<style scoped>
.w-50 {
  width: 20rem;
  margin: 1rem
}

.my-header {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
}
.el-pagination{
  position: fixed;
  bottom:12%;
  left:50%;
}
</style>
