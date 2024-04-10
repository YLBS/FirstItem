<template>
  <el-table :data="tableData" style="width: 100%" v-if="!loading" v-loading="loading">
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

        <el-button size="small" @click="examine(scope.row.CarId)">查看</el-button>
      </template>
    </el-table-column>

    <el-table-column label="State">
      <template #default="scope">
        <el-button size="small" type="danger" @click="handCheck(scope.row.CarId)" v-if="scope.row.state == '待审核'">{{
          scope.row.state }}</el-button>
        <el-tag v-if="scope.row.state != '待审核'">{{ scope.row.state }}</el-tag>
      </template>
    </el-table-column>

  </el-table>
  <el-drawer v-model="drawer" title="上架审核" :direction="direction" :before-close="handleClose" id="Test">

    <el-form ref="ruleFormRef" :model="inputCar" label-width="110px" :inline="true" status-icon>

      <el-form-item label="车主" prop="Proposer" required class="w-50">
        <el-input v-model="inputCar.Proposer" disabled />
      </el-form-item>

      <el-form-item label="品牌名称" prop="BrandName" required class="w-50">
        <el-input v-model="inputCar.BrandName" disabled />
      </el-form-item>
      <el-form-item label="车型" prop="Name" required class="w-50">
        <el-input v-model="inputCar.Name" disabled />
      </el-form-item>
      <el-form-item label="车源地" prop="VehicleSource" required class="w-50">
        <el-input v-model="inputCar.VehicleSource" disabled />
      </el-form-item>

      <el-form-item label="车身颜色" prop="VehicleColor" required class="w-50">
        <el-input v-model="inputCar.VehicleColor" disabled />
      </el-form-item>
      <el-form-item label="过户次数" prop="TransfersNumber" required class="w-50">
        <el-input v-model="inputCar.TransfersNumber" type="text" autocomplete="off" disabled />
      </el-form-item>
      <el-form-item label="变速箱类型" prop="TransmissionType" required class="w-50">
        <el-input v-model="inputCar.TransmissionType" disabled />
      </el-form-item>

      <el-form-item label="燃油类型" prop="Fuel" required class="w-50">
        <el-select v-model="inputCar.Fuel" disabled>
          <el-option label="汽油" value="汽油" />
        </el-select>
      </el-form-item>
      <el-form-item label="座位数" prop="Seating" required class="w-50">
        <el-select v-model="inputCar.Seating" disabled>
          <el-option label="1" value="1" />
        </el-select>
      </el-form-item>
      <el-form-item label="上牌时间" required class="w-50">
        <el-input v-model="inputCar.RegistrationTime" disabled />
      </el-form-item>


      <el-form-item label="车龄" prop="vehicleAge" required class="w-50">
        <el-input v-model.number="inputCar.vehicleAge" type="text" autocomplete="off" disabled />
      </el-form-item>

      <el-form-item label="公里数" prop="Mileage" required class="w-50">
        <el-input v-model="inputCar.Mileage" type="text" autocomplete="off" style="width: 150px;" disabled />
        <span style="margin-left: 8px;"> 单位/万</span>
      </el-form-item>
      <el-form-item label="类型" prop="VehicleLevelName" required class="w-50">
        <el-select v-model="inputCar.VehicleLevelName" multiple collapse-tags collapse-tags-tooltip>
          <el-option label="SUV" value="SUV" />
          <el-option label="MPV" value="MPV" />
          <el-option label="MPV1" value="MPV1" />
          <el-option label="MPV2" value="MPV2" />
          <el-option label="练手车1" value="练手车1" />
          <el-option label="练手车" value="练手车" />
        </el-select>
      </el-form-item>

      <el-form-item label="售价" prop="SellingPrice" required class="w-50">
        <el-input v-model="inputCar.SellingPrice" disabled style="width: 150px;" />
        <span style="margin-left: 8px;"> 单位/万</span>
      </el-form-item>

      <el-form-item label="是否支持分期" prop="Amortize" required class="w-50">
        <el-switch v-model="inputCar.Amortize" disabled />
      </el-form-item>
      <el-form-item label="首付" prop="DownPayment" required class="w-50">
        <el-input v-model="inputCar.DownPayment" disabled style="width: 150px;" />
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
        <el-input v-model="inputCar.VehicleCondition" type="textarea" :autosize="{ minRows: 4, maxRows: 6 }" disabled />
      </el-form-item>
      <el-form-item>
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
</template>

<script lang="ts" setup>
import { Timer } from '@element-plus/icons-vue'
import { ref, onMounted, reactive } from 'vue'
import { ElMessageBox, ElMessage, UploadProps } from 'element-plus'
import Qs from 'qs'

import type { UploadFile } from 'element-plus'

import { GeConOrder as GeConOrderApi, GetCarInfo as GetCarInfoApi } from '@/ts/RequestInterface';

interface User {
  date: string
  CarId: string
  state: string
  condition: string
}
interface FileData {
  name: string
  url: string
}
const inputCar = reactive({
  vehicleAge: '',
  BrandName: '',
  Name: '',
  TransmissionType: '',
  Mileage: '',
  Seating: '',
  Fuel: '',
  RegistrationTime: '',//上牌日期
  VehicleSource: '',//车源地
  VehicleColor: '',
  TransfersNumber: '',//过户次数
  VehicleLevelName: '',//汽车类型
  Registration: '',//行驶证
  Appearance: '',//外观
  VehiTrim: '',
  VehicleCondition: '',//描述
  SellingPrice: '',//售价
  Amortize: false,//是否分期
  DownPayment: '',//首付
  Proposer:''
})

const drawer = ref(false)
const visible = ref(false)

const direction = ref('rtl')
const handleClose = (done: () => void) => {
  done()
}
const loading = ref(true)
const textarea2 = ref('')



let fileList = reactive<[FileData]>([
  {
    name: 'food.jpeg',
    url: 'https://fuss10.elemecdn.com/3/63/4e7f3a15429bfda99bce42a18cdd1jpeg.jpeg?imageMogr2/thumbnail/360x360/format/webp/quality/100',
  }
])
let fileList1 = reactive<[FileData]>([
  {
    name: 'food.jpeg',
    url: 'https://fuss10.elemecdn.com/3/63/4e7f3a15429bfda99bce42a18cdd1jpeg.jpeg?imageMogr2/thumbnail/360x360/format/webp/quality/100',
  }
])
let fileList2 = reactive<[FileData]>([
  {
    name: 'food.jpeg',
    url: 'https://fuss10.elemecdn.com/3/63/4e7f3a15429bfda99bce42a18cdd1jpeg.jpeg?imageMogr2/thumbnail/360x360/format/webp/quality/100',
  }
])
const dialogImageUrl = ref('')
const dialogVisible = ref(false)


const handlePictureCardPreview: UploadProps['onPreview'] = (uploadFile) => {
  dialogImageUrl.value = uploadFile.url!
  dialogVisible.value = true
}

const handleEdit = (index: number, row: User) => {
  console.log(index, row)
}



const handCheck = async (Id: number) => {
  //alert(Id)
  fileList.splice(0);
  fileList1.splice(0);
  fileList2.splice(0);
  
  const data = { Id: Id }
  const da = await GetCarInfoApi(Qs.stringify(data));
  inputCar.Proposer=da.data.proposer;
  inputCar.BrandName = da.data.brandName;
  inputCar.Name = da.data.name;
  inputCar.VehicleSource = da.data.vehicleSource;
  inputCar.VehicleColor = da.data.vehicleColor
  inputCar.TransfersNumber = da.data.transfersNumber
  inputCar.Fuel = da.data.fuel;
  inputCar.Seating = da.data.seating;
  inputCar.RegistrationTime = da.data.registrationTime
  inputCar.vehicleAge = da.data.vehicleAge
  inputCar.SellingPrice = da.data.sellingPrice
  inputCar.Amortize = da.data.amortize
  inputCar.Registration = da.data.registration//行驶证
  inputCar.Appearance = da.data.appearance //外观
  inputCar.VehiTrim = da.data.vehiTrim //内饰
  inputCar.Mileage = da.data.mileage
  inputCar.DownPayment = da.data.downPayment
  inputCar.VehicleCondition = da.data.vehicleCondition
  inputCar.TransmissionType=da.data.transmissionType
  //console.log(da.data);
  if (inputCar.Registration != null) {
    var AVR = inputCar.Registration.split(";")
    for (let i = 0; i < AVR.length; i++) {
      //console.log(AVR[i]);
      if (AVR[i] != '') {
        fileList[i] = {
          name: "123",
          url: "http://localhost:5246/" + AVR[i]
        }
      }
    }
  }
  if (inputCar.Appearance != null) {
    var AVR = inputCar.Appearance.split(";")
    for (let i = 0; i < AVR.length; i++) {
      //console.log(AVR[i]);
      if (AVR[i] != '') {
        fileList1[i] = {
          name: "123",
          url: "http://localhost:5246/" + AVR[i]
        }
      }
    }
  }

  if (inputCar.VehiTrim != null) {
    var AVR = inputCar.VehiTrim.split(";")
    for (let i = 0; i < AVR.length; i++) {
      //console.log(AVR[i]);
      if (AVR[i] != '') {
        fileList2[i] = {
          name: "123",
          url: "http://localhost:5246/" + AVR[i]
        }
        //console.log(fileList1[i]);
      }
    }
  }
  drawer.value = true
}
const Disagree = () => {
  textarea2.value = '';
  visible.value = true;
}
const confirmDisagree = () => {
  //visible.value = true;
  if (textarea2.value == '') {
    ElMessage.error('请填写不同意的原因');
  }
  else {  //调用接口提交
    drawer.value = false;
    visible.value = false;
  }
}
const examine = (carId: number) => {
  alert(carId);
}
const Agree = () => {
  if (inputCar.VehicleLevelName == '') {
    ElMessage.error('请补充汽车类型');
  }
  else {
    ElMessage({
      message: '提交成功',
      type: 'success',
    })
  }
}
const tableData: User[] = [
  
]

tableData[0] = {
  date: "2024-01-08",
  CarId: "11",
  state: "已通过审核",
  condition: ""
}

onMounted(async () => {
  var datas = await GeConOrderApi(1);
  for (var i = 0; i < datas.data.length; i++) {
    tableData[i] = {
      date: datas.data[i].createTime,
      CarId: datas.data[i].carInfoId,
      state: datas.data[i].state,
      condition: datas.data[i].condition
    }
    //console.log(datas.data[i]);
  }
  loading.value = false

})


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
</style>
<style>
#Test .el-upload--picture-card {
  display: none;
}
</style>