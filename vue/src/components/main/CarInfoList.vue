<template>
    <div v-loading="loading">
        <el-table ref="tableRef" row-key="id" :data="tableData.newtable" style="width: 100%"
            @cell-dblclick="doubleClick">
            <el-table-column type="index" :index="indexMethod" />
            <el-table-column prop="brandName" label="品牌" width="170" />
            <el-table-column prop="name" label="车名" width="170" />
            <el-table-column prop="vehicleAge" label="年份" width="170" />
            <el-table-column prop="sellingPrice" label="价格" width="170" />
            <el-table-column prop="state" label="状态" width="170" />

            <el-table-column fixed="right" width="210">
                <template #header>

                    <el-button type="info" plain size="small" style="margin-left:5px"
                        @click="addDialogFormVisible = true">添加</el-button>
                </template>

                <template #default="scope">
                    <!--  <el-button size="small" @click="">撤销申请</el-button> -->
                    <el-button size="small" type="danger" @click="soldOut(scope.row.id)"
                        v-if="scope.row.state == '热售中'">下架</el-button>
                    <el-button size="small" type="success" @click="" v-if="scope.row.state == '审核中'">撤销申请</el-button>
                </template>
            </el-table-column>
        </el-table>
        <div class="el-pagination">
            <el-pagination layout="prev, pager, next" :total="totalNumber" @current-change="currentChange" />
        </div>

        <el-dialog v-model="addDialogFormVisible" title="添加" width="1100" draggable>
            <ConsignmentOrder @refresh="refresh"/>
        </el-dialog>
        <el-dialog v-if="dialogFormVisible" v-model="dialogFormVisible" title="详情" width="1100" draggable>
            <el-form ref="ruleFormRef" :model="inputCar" label-width="110px" :inline="true" status-icon  id="Test">

                <el-form-item label="车主" prop="Proposer" class="w-50">
                    <el-input v-model="inputCar.proposer" disabled />
                </el-form-item>

                <el-form-item label="品牌名称" prop="BrandName" class="w-50">
                    <el-input v-model="inputCar.brandName" disabled />
                </el-form-item>
                <el-form-item label="车型" prop="Name" class="w-50">
                    <el-input v-model="inputCar.name" disabled />
                </el-form-item>
                <el-form-item label="车源地" prop="VehicleSource" class="w-50">
                    <el-input v-model="inputCar.vehicleSource" disabled />
                </el-form-item>

                <el-form-item label="车身颜色" prop="VehicleColor" class="w-50">
                    <el-input v-model="inputCar.vehicleColor" disabled />
                </el-form-item>
                <el-form-item label="过户次数" prop="TransfersNumber" class="w-50">
                    <el-input v-model="inputCar.transfersNumber" type="text" autocomplete="off" disabled />
                </el-form-item>
                <el-form-item label="变速箱类型" prop="TransmissionType" class="w-50">
                    <el-input v-model="inputCar.transmissionType" disabled />
                </el-form-item>
                <el-form-item label="燃油类型" prop="Fuel" class="w-50">
                    <el-input v-model="inputCar.fuel" disabled />
                </el-form-item>
                <el-form-item label="座位数" prop="Seating" class="w-50">
                    
                    <el-input v-model="inputCar.seating" disabled />
                </el-form-item>
                <el-form-item label="上牌时间" class="w-50">
                    <el-input v-model="inputCar.registrationTime" disabled />
                </el-form-item>


                <el-form-item label="车龄" prop="vehicleAge" class="w-50">
                    <el-input v-model.number="inputCar.vehicleAge" type="text" autocomplete="off" disabled />
                </el-form-item>

                <el-form-item label="公里数" prop="Mileage" class="w-50">
                    <el-input v-model="inputCar.mileage" type="text" autocomplete="off" style="width: 150px;"
                        disabled />
                    <span style="margin-left: 8px;"> 单位/万</span>
                </el-form-item>
                <el-form-item label="类型" prop="vehicleLevelName" class="w-50" >
                    <el-input v-model="inputCar.vehicleLevelName" type="text" autocomplete="off" style=""
                        disabled />
                </el-form-item>
                <el-form-item label="售价" prop="SellingPrice" class="w-50">
                    <el-input v-model="inputCar.sellingPrice" disabled style="width: 150px;" />
                    <span style="margin-left: 8px;"> 单位/万</span>
                </el-form-item>
                <el-form-item label="首付" prop="DownPayment" class="w-50">
                    <el-input v-model="inputCar.downPayment" disabled style="width: 150px;" />
                    <span style="margin-left: 8px;"> 单位/万</span>
                </el-form-item>
                <el-form-item label="是否支持分期" prop="Amortize" required class="w-50">
                    <el-switch v-model="inputCar.amortize" disabled />
                </el-form-item>
               <br/>
                <el-form-item label="行驶证" prop="Registration" required class="w-50">

                    <el-upload v-model:file-list="fileList" list-type="picture-card"
                        :on-preview="handlePictureCardPreview" :disabled="true">
                    </el-upload>

                    <el-dialog v-model="dialogVisible">
                        <img w-full :src="dialogImageUrl" alt="Preview Image" width="400px" />
                    </el-dialog>
                </el-form-item>

                <el-form-item label="外观" prop="Appearance" required class="w-50">

                    <el-upload v-model:file-list="fileList1" list-type="picture-card"
                        :on-preview="handlePictureCardPreview" :disabled="true">
                    </el-upload>

                </el-form-item>

                <el-form-item label="内饰" prop="VehiTrim" required class="w-50">

                    <el-upload v-model:file-list="fileList2" list-type="picture-card"
                        :on-preview="handlePictureCardPreview" :disabled="true">
                    </el-upload>

                </el-form-item>

                <el-form-item label="车况" prop="VehicleCondition" style="width: 90%;">
                    <el-input v-model="inputCar.vehicleCondition" type="textarea" :autosize="{ minRows: 4, maxRows: 6 }"
                        disabled />
                </el-form-item>
                

            </el-form>
        </el-dialog>
    </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, reactive } from 'vue'
import ConsignmentOrder from '@/components/main/ConsignmentOrder.vue'
import { useRouter } from 'vue-router'

import { ElMessage, UploadProps } from 'element-plus'
import type { TableInstance } from 'element-plus'

import { GetCarListByUserId as GetCarListApi, SoldOutById, baseURL, GetCarInfo as GetCarInfoApi } from '@/ts/RequestInterface'
import Qs from 'qs'

const Emits = defineEmits(["refresh"]);
const indexMethod = (index: number) => {
    return index + 1
}

const addDialogFormVisible = ref(false)
const dialogFormVisible = ref(false)
const loading = ref(true)
const router = useRouter();

let inputCar = reactive({
    vehicleAge: '',
    brandName: '',
    name: '',
    transmissionType: '',
    mileage: '',
    seating: '',
    vehicleLevelName: '',
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
interface FileData {
    name: string
    url: string
}
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

let conId = ref(0);

const dialogImageUrl = ref('')
const dialogVisible = ref(false)
const handlePictureCardPreview: UploadProps['onPreview'] = (uploadFile) => {
  dialogImageUrl.value = uploadFile.url!
  dialogVisible.value = true
}
const doubleClick = async (row: any, column: any, cell: any, even: any) => {

    //console.log(row.id);
    fileList.splice(0);
    fileList1.splice(0);
    fileList2.splice(0);
    conId.value = row.id;
    const data = { Id: row.id }
    inputCar = (await GetCarInfoApi(Qs.stringify(data))).data;

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
    dialogFormVisible.value = true;
    //let ob = JSON.stringify({ id: row.id })
    //router.push({ name: 'NewSalesTicket', state: { ob } })
}

const tableRef = ref<TableInstance>();

let tableData = reactive({
    newtable: [{
        id: 0,
        brandName: '红旗',
        name: '红旗H5',
        vehicleAge: "",
        mileage: '12343',
        sellingPrice: "",
        state: ''
    }]
})
const refresh=async (page:number)=>{
    var data = await GetCarListApi(page);
    tableData.newtable = data.data
    totalNumber.value = data.tatleCount;
    
}
const currentChange=async (pages:number)=>{
    refresh(pages);
    console.log(pages);
    
  //currentPage.value=pager;
}
const totalNumber = ref(0);

onMounted(async () => {
    //var data = await GetCarListApi(1);
    // console.log(data);
    await refresh(1);
   
    //totalNumber.value = 10;
    loading.value = false
})
const soldOut = async (Id: any) => {
    // console.log(Id);
    const result = await SoldOutById(Id);
    console.log(result);
    if (result) {
        ElMessage({
            message: '成功下架',
            type: 'success',
        })
        var data = await GetCarListApi(1);
        tableData.newtable = data.data
    }
    else {

        ElMessage.error('错误');
    }
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