<template>
    <div class="bigDiv" v-loading="loading">
        <div style="width: 97px;margin: 0 auto;">
            <h2>售卖合同</h2>
        </div>
        <el-descriptions class="margin-top" title="卖家信息" :column="3" border>
            <el-descriptions-item>
                <template #label>
                    <div class="cell-item">
                        <el-icon :style="iconStyle">
                            <user />
                        </el-icon>
                        姓名
                    </div>
                </template>
                {{ sales.data.sellerName }}
            </el-descriptions-item>
            <el-descriptions-item>

                <template #label>
                    <div class="cell-item">
                        <el-icon :style="iconStyle">
                            <iphone />
                        </el-icon>
                        电话
                    </div>
                </template>
                {{ sales.data.sellerPhone }}
            </el-descriptions-item>
            <el-descriptions-item>

                <template #label>
                    <div class="cell-item">
                        <el-icon :style="iconStyle">
                            <View />
                        </el-icon>
                        银行卡号码
                    </div>
                </template>
                {{ sales.data.sellerCardNumber }}
            </el-descriptions-item>
        </el-descriptions>

        <el-descriptions class="margin-top" title="买家信息" :column="3" border>
            <el-descriptions-item>

                <template #label>
                    <div class="cell-item">
                        <el-icon :style="iconStyle">
                            <user />
                        </el-icon>
                        姓名
                    </div>
                </template>
                {{ sales.data.buyerName }}
            </el-descriptions-item>
            <el-descriptions-item>

                <template #label>
                    <div class="cell-item">
                        <el-icon :style="iconStyle">
                            <iphone />
                        </el-icon>
                        电话
                    </div>
                </template>
                {{ sales.data.buyerPhone }}
            </el-descriptions-item>
            <el-descriptions-item>

                <template #label>
                    <div class="cell-item">
                        <el-icon :style="iconStyle">
                            <View />
                        </el-icon>
                        银行卡号码
                    </div>
                </template>
                {{ sales.data.buyerCardNumber }}
            </el-descriptions-item>
        </el-descriptions>

        <el-descriptions class="margin-top" title="汽车信息" :column="4" border>
            <el-descriptions-item>

                <template #label>
                    <div class="cell-item">
                        <el-icon :style="iconStyle">
                            <user />
                        </el-icon>
                        汽车名称
                    </div>
                </template>
                {{ sales.data.carName }}
            </el-descriptions-item>

            <el-descriptions-item>

                <template #label>
                    <div class="cell-item">
                        <el-icon :style="iconStyle">
                            <Location />
                        </el-icon>
                        车源地
                    </div>
                </template>
                {{ sales.data.vehicleSource }}
            </el-descriptions-item>

            <el-descriptions-item>

                <template #label>
                    <div class="cell-item">
                        <el-icon :style="iconStyle">
                            <Location />
                        </el-icon>
                        总价
                    </div>
                </template>
                {{ sales.data.totalPrices }}/万
            </el-descriptions-item>

            <el-descriptions-item>

                <template #label>
                    <div class="cell-item">
                        <el-icon :style="iconStyle">
                            <location />
                        </el-icon>
                        汽车编号
                    </div>
                </template>
                <div style="width: 50px;">
                    <span style="cursor: pointer; text-decoration: underline;" @click="examine(sales.data.carId)">
                        {{ sales.data.carId }}
                    </span>
                </div>

            </el-descriptions-item>
        </el-descriptions>

        <h4>审核意见</h4>
        <el-timeline style="max-width: 520px" >
            <el-timeline-item center :timestamp="sales.data.createTime" placement="top">
                提交
            </el-timeline-item>
            <el-timeline-item center :timestamp="sales.data.time1" placement="top">
                <el-card>
                    <h4> {{ sales.data.sellerName }}</h4>
                    <el-select style="width:100px" v-model="sales.data.idea1" :disabled="sales.data.edit1 ">
                        <el-option label="同意" value="同意" />
                        <el-option label="不同意" value="不同意" />
                    </el-select>
                    <el-input style="width: 240px; margin-left: 15px;" :rows="2" type="textarea" placeholder="若不通过请填写"  v-model="sales.data.reason1" :disabled="sales.data.edit1 "/>
                    <!-- <p>待审核</p> -->
                </el-card>
            </el-timeline-item>
            <el-timeline-item v-if="secondLevel" :timestamp="sales.data.time2" placement="top">
                    <el-card>
                    <h4> {{ sales.data.name }}</h4>
                    <el-select style="width:100px"   v-model="sales.data.idea2" :disabled="sales.data.edit2 ">
                        <el-option label="同意" value="同意" />
                        <el-option label="不同意" value="不同意" />
                    </el-select>
                    <el-input style="width: 240px; margin-left: 15px;" :rows="2" type="textarea" placeholder="若不通过请填写"  v-model="sales.data.reason2" :disabled="sales.data.edit2 "/>
                    <!-- <p>待审核</p> -->
                </el-card>
                </el-timeline-item>

                <el-timeline-item  :timestamp="sales.data.endTime" placement="top" class="ttt">
                    结束
                </el-timeline-item>
            <template v-if="false">
                
            </template>

        </el-timeline>
        <el-form-item class="headPortrait" v-if="!sales.data.edit1 || !sales.data.edit2">
            <el-button type="primary" v-if="sales.data.edit1"  @click="submit2">提交</el-button>
            <el-button type="primary" v-else @click="submit1">提交</el-button>
        </el-form-item>
    </div>

    <el-drawer v-model="drawer" title="汽车详细信息" :direction="direction" :before-close="handleClose" id="Test">

        <el-form ref="ruleFormRef" :model="inputCar" label-width="110px" :inline="true" status-icon>

            <el-form-item label="车主" prop="Proposer" required class="w-50">
                <el-input v-model="inputCar.proposer" disabled />
            </el-form-item>

            <el-form-item label="品牌名称" prop="BrandName" required class="w-50">
                <el-input v-model="inputCar.brandName" disabled />
            </el-form-item>
            <el-form-item label="车型" prop="Name" required class="w-50">
                <el-input v-model="inputCar.name" disabled />
            </el-form-item>
            <el-form-item label="车源地" prop="VehicleSource" required class="w-50">
                <el-input v-model="inputCar.vehicleSource" disabled />
            </el-form-item>

            <el-form-item label="车身颜色" prop="VehicleColor" required class="w-50">
                <el-input v-model="inputCar.vehicleColor" disabled />
            </el-form-item>
            <el-form-item label="过户次数" prop="TransfersNumber" required class="w-50">
                <el-input v-model="inputCar.transfersNumber" type="text" autocomplete="off" disabled />
            </el-form-item>
            <el-form-item label="变速箱类型" prop="TransmissionType" required class="w-50">
                <el-input v-model="inputCar.transmissionType" disabled />
            </el-form-item>

            <el-form-item label="燃油类型" prop="Fuel" required class="w-50">
                <el-select v-model="inputCar.fuel" disabled>
                    <el-option label="汽油" value="汽油" />
                </el-select>
            </el-form-item>
            <el-form-item label="座位数" prop="Seating" required class="w-50">
                <el-select v-model="inputCar.seating" disabled>
                    <el-option label="1" value="1" />
                </el-select>
            </el-form-item>
            <el-form-item label="上牌时间" required class="w-50">
                <el-input v-model="inputCar.registrationTime" disabled />
            </el-form-item>


            <el-form-item label="车龄" prop="vehicleAge" required class="w-50">
                <el-input v-model.number="inputCar.vehicleAge" type="text" autocomplete="off" disabled />
            </el-form-item>

            <el-form-item label="公里数" prop="Mileage" required class="w-50">
                <el-input v-model="inputCar.mileage" type="text" autocomplete="off" style="width: 150px;" disabled />
                <span style="margin-left: 8px;"> 单位/万</span>
            </el-form-item>
            <el-form-item label="类型" prop="VehicleLevelName" required class="w-50">
                <el-select v-model="inputCar.vehicleLevelName" disabled>
                </el-select>
            </el-form-item>
            <el-form-item label="售价" prop="SellingPrice" required class="w-50">
                <el-input v-model="inputCar.sellingPrice" disabled style="width: 150px;" />
                <span style="margin-left: 8px;"> 单位/万</span>
            </el-form-item>

            <el-form-item label="是否支持分期" prop="Amortize" required class="w-50">
                <el-switch v-model="inputCar.amortize" disabled />
            </el-form-item>
            <el-form-item label="首付" prop="DownPayment" required class="w-50">
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
                <el-input v-model="inputCar.vehicleCondition" type="textarea" :autosize="{ minRows: 4, maxRows: 6 }"
                    disabled />
            </el-form-item>

        </el-form>

    </el-drawer>
</template>

<script lang="ts" setup>

import { baseURL, GetCarInfo as GetCarInfoApi,GetSalesTicketBySaleId,UpSalesAndCarState } from '@/ts/RequestInterface';

import Qs from 'qs'
import { computed, ref, reactive, onMounted } from 'vue'
import {
    Iphone,
    View,
    User,
    Location,
} from '@element-plus/icons-vue'

import {  ElMessage, UploadProps } from 'element-plus'
const loading = ref(true);
let sales = reactive({
    data: {
        sellerId: 0,
        sellerName: "",
        sellerPhone: "",
        sellerCardNumber: "",
        buyerId: 0,
        buyerName: "",
        buyerPhone: "",
        buyerCardNumber: "",
        carId: 0,
        carName: "",
        totalPrices: 0,
        vehicleSource: "",
        createTime:Date,
        submit:false,

        time1:Date,
        reason1:"",
        idea1:"",
        name:"",
        time2:Date,
        reason2:"",
        idea2:"",
        endTime:"",
        state:"",
        edit1:false,
        edit2:false
    },

})

const iconStyle = computed(() => {
    const marginMap = {
        large: '8px',
        default: '6px',
        small: '4px',
    }
    return {
        marginRight: marginMap.default,
    }
})
const drawer = ref(false)

const direction = ref('rtl')
let inputCar = reactive({
    id: 0,
    vehicleAge: '',
    brandName: '',
    name: '',
    transmissionType: '',
    mileage: '',
    seating: '',
    fuel: '',
    registrationTime: '',//上牌日期
    vehicleSource: '',//车源地
    vehicleColor: '',
    transfersNumber: '',//过户次数
    vehicleLevelName: '',//汽车类型
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
        url: '@/assets/2.jpg',
    }
])
let fileList1 = reactive<[FileData]>([
    {
        name: 'food.jpeg',
        url: '@/assets/2.jpg',
    }
])
let fileList2 = reactive<[FileData]>([
    {
        name: 'food.jpeg',
        url: '@/assets/2.jpg',
    }
])
const dialogImageUrl = ref('')
const dialogVisible = ref(false)


const handleClose = (done: () => void) => {
    done()
}
const handlePictureCardPreview: UploadProps['onPreview'] = (uploadFile) => {
    dialogImageUrl.value = uploadFile.url!
    dialogVisible.value = true
}

const examine = async (CId: number) => {
    fileList.splice(0);
    fileList1.splice(0);
    fileList2.splice(0);

    const data = { Id: CId }
    inputCar = (await GetCarInfoApi(Qs.stringify(data))).data;

    if (inputCar.registration != null) {
        var AVR = inputCar.registration.split(";")
        for (let i = 0; i < AVR.length; i++) {
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
const nn = reactive({
    gg: "gg"
})

const secondLevel= ref(sales.data.totalPrices>8?true:false);
onMounted(async () => {
    const id = JSON.parse(history.state.ob).id;
    var data = (await GetSalesTicketBySaleId(id)).data;
    sales.data = data;
    nn.gg = data.buyerName;
    console.log(sales.data);
    loading.value = false;
    secondLevel.value= sales.data.totalPrices>8?true:false;
    console.log(secondLevel);
    
})
const condition={
    id:0,
    carId:0,
    reason1:"",
    idea1:"",
    idea2:"",
    reason2:""
}

const submit1=async ()=>{
    if(sales.data.idea1==null){
        ElMessage.error("请选择意见")
        return;
    }
    condition.id=sales.data.sellerId;
    condition.carId=sales.data.carId;
    condition.idea1=sales.data.idea1;
    condition.reason1=sales.data.reason1;
    const re=(await UpSalesAndCarState(condition,1,secondLevel.value));
    console.log(re);
}
const submit2=async ()=>{
    if(sales.data.idea2==null){
        ElMessage.error("请选择意见")
        return;
    }
    condition.id=sales.data.sellerId;
    condition.carId=sales.data.carId;
    condition.idea2=sales.data.idea2;
    condition.reason2=sales.data.reason2;
    const re=(await UpSalesAndCarState(condition,2,true));
    console.log(re);
}

</script>

<style scoped>
.el-descriptions {
    margin-top: 20px;
}

.cell-item {
    display: flex;
    align-items: center;
}

.margin-top {
    margin-top: 20px;
}

.bigDiv {

    margin: 0 10%;
}

.headPortrait {
    cursor: pointer;
    width: 100px;
    height: 100px;
    margin: 0 auto
}
</style>

<style>
.el-timeline li:nth-of-type(2) .el-timeline-item__node--normal {
    background-color: red;
}
</style>
