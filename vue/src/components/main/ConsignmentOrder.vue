<template>
    <el-form ref="ruleFormRef" :model="inputCar" :rules="rules" label-width="110px" :inline="true" status-icon>
        <el-form-item label="品牌名称" prop="BrandName" class="w-50 m-2 allBrands">
            <el-input v-model="inputCar.BrandName" clearable class="" />
            <div class="brands">
                <div class="group" v-for="(intem, index) in brand_names" :key="index">
                    <p class="capital" style="color:red"> {{ intem.letter }}</p>
                    <a class="brandName" v-for="(carName, index) in intem.name" :key="index"
                        @click="clickBrands(carName)">
                        <span>
                            {{ carName }}</span>
                    </a>
                </div>

            </div>
        </el-form-item>
        <el-form-item label="车型" prop="Name" class="w-50 m-2">
            <el-input v-model="inputCar.Name" clearable placeholder="如：宝马三系" />
        </el-form-item>
        <el-form-item label="车源地" prop="VehicleSource" class="w-50 m-2">
            <el-input v-model="inputCar.VehicleSource" clearable placeholder="如：桂林市" />
        </el-form-item>


        <el-form-item label="车身颜色" prop="VehicleColor" class="w-50 m-2">
            <el-input v-model="inputCar.VehicleColor" clearable />
        </el-form-item>

        <el-form-item label="过户次数" prop="TransfersNumber" class="w-50 m-2">
            <el-input v-model.number="inputCar.TransfersNumber" type="text" autocomplete="off" />
        </el-form-item>

        <el-form-item label="变速箱类型" prop="TransmissionType" class="w-50 m-2">
            <el-select v-model="inputCar.TransmissionType">
                <el-option label="手动" value="手动" />
                <el-option label="自动" value="自动" />
            </el-select>
        </el-form-item>
        <el-form-item label="燃油类型" prop="Fuel" class="w-50 m-2">
            <el-select v-model="inputCar.Fuel">
                <el-option label="汽油" value="汽油" />
                <el-option label="电动" value="电动" />
                <el-option label="油电混动" value="油电混动" />
            </el-select>
        </el-form-item>
        <el-form-item label="座位数" prop="Seating" class="w-50 m-2">
            <el-select-v2 v-model.number="inputCar.Seating" :options="options" />
        </el-form-item>
        <el-form-item label="上牌时间" required class="w-50 m-2">
            <el-col :span="11">
                <el-form-item prop="RegistrationTime">
                    <el-date-picker v-model="inputCar.RegistrationTime" label="Pick a date" placeholder="Pick a date"
                        :disabled-date="disabledDate" />
                </el-form-item>
            </el-col>
        </el-form-item>

        <el-form-item label="车龄" prop="vehicleAge" class="w-50 m-2">
            <el-input v-model.number="inputCar.vehicleAge" type="text" autocomplete="off" />
        </el-form-item>

        <el-form-item label="公里数" prop="Mileage" class="w-50 m-2">
            <el-input-number v-model="inputCar.Mileage" type="text" autocomplete="off"
                placeholder="The unit is ten thousands" />
            <span style="margin-left: 8px;"> 单位/万</span>
        </el-form-item>
        <el-form-item label="类型" prop="VehicleLevelName" class="w-50 m-2">
            <el-select v-model="inputCar.VehicleLevelName" disabled>
                <el-option label="SUV" value="SUV" />
                <el-option label="MPV" value="MPV" />
                <el-option label="练手车" value="练手车" />
            </el-select>
        </el-form-item>

        <el-form-item label="售价" prop="SellingPrice" class="w-50 m-2">
            <el-input-number v-model.number="inputCar.SellingPrice" type="text" autocomplete="off" :precision="2"
                :step="0.1" controls-position="right" />
            <span style="margin-left: 8px;"> 单位/万</span>
        </el-form-item>

        <el-form-item label="是否支持分期" prop="Amortize" class="w-50 m-2">
            <el-switch v-model="inputCar.Amortize" />
        </el-form-item>
        <el-form-item label="首付" prop="DownPayment" class="w-50 m-2">
            <el-input-number v-model="inputCar.DownPayment" type="text" autocomplete="off" :precision="2" :step="0.1"
                controls-position="right" />
            <span style="margin-left: 8px;"> 单位/万</span>
        </el-form-item>

        <el-form-item label="上传行驶证" prop="Registration" style="width: 90%;">
            <el-upload :class="'upload_wrapper ' + (RhideUpload ? 'hide' : '')" list-type="picture-card" :limit="1"
                :file-list="RFileList" :show-file-list="true" :on-change="RfileChange" :auto-upload="false"
                accept="image/*" :http-request="VehicleConditionFile" :disabled="RhideUpload">
                <el-icon>
                    <Plus />
                </el-icon>
                <template #file="{ file }">
                    <div>
                        <img class="el-upload-list__item-thumbnail" :src="file.url" alt="123" />
                        <span class="el-upload-list__item-actions">
                            <span class="el-upload-list__item-preview" @click="handlePictureCardPreview(file)">
                                <el-icon>
                                    <zoom-in />
                                </el-icon>
                            </span>
                            <span class="el-upload-list__item-delete" @click="RhandleRemove(file)">
                                <el-icon>
                                    <Delete />
                                </el-icon>
                            </span>
                        </span>
                    </div>
                </template>
            </el-upload>
            <el-dialog v-model="dialogVisible">
                <img w-full class="full_img" :src="dialogImageUrl" alt="Preview Image" style="width:400px" />
            </el-dialog>
        </el-form-item>
        <el-form-item label="上传外观图片" prop="Appearance" style="width: 90%;">
            <el-upload :class="'upload_wrapper ' + (AhideUpload ? 'hide' : '')" list-type="picture-card" :limit="20"
                :file-list="AFileList" :show-file-list="true" :on-change="AfileChange" :auto-upload="false" multiple
                accept="image/*" :http-request="AppearanceFile" :disabled="AhideUpload">
                <el-icon>
                    <Plus />
                </el-icon>

                <template #file="{ file }">
                    <div>
                        <img class="el-upload-list__item-thumbnail" :src="file.url" alt="123" />
                        <span class="el-upload-list__item-actions">
                            <span class="el-upload-list__item-preview" @click="handlePictureCardPreview(file)">
                                <el-icon>
                                    <zoom-in />
                                </el-icon>
                            </span>
                            <span class="el-upload-list__item-delete" @click="AhandleRemove(file)">
                                <el-icon>
                                    <Delete />
                                </el-icon>
                            </span>
                        </span>
                    </div>
                </template>
            </el-upload>
        </el-form-item>
        <el-form-item label="上传内饰图片" prop="VehiTrim" style="width: 90%;">
            <el-upload :class="'upload_wrapper ' + (VhideUpload ? 'hide' : '')" list-type="picture-card" :limit="20"
                :file-list="VFileList" :show-file-list="true" :on-change="VfileChange" :auto-upload="false"
                accept="image/*" :http-request="VehiTrimFile" :disabled="VhideUpload" multiple>
                <el-icon>
                    <Plus />
                </el-icon>

                <template #file="{ file }">
                    <div>
                        <img class="el-upload-list__item-thumbnail" :src="file.url" alt="123" />
                        <span class="el-upload-list__item-actions">
                            <span class="el-upload-list__item-preview" @click="handlePictureCardPreview(file)">
                                <el-icon>
                                    <zoom-in />
                                </el-icon>
                            </span>
                            <span class="el-upload-list__item-delete" @click="VhandleRemove(file)">
                                <el-icon>
                                    <Delete />
                                </el-icon>
                            </span>
                        </span>
                    </div>
                </template>
            </el-upload>
        </el-form-item>

        <el-form-item label="车况" prop="VehicleCondition" style="width: 90%;">
            <el-input v-model="inputCar.VehicleCondition" type="textarea" :autosize="{ minRows: 4, maxRows: 6 }" />
        </el-form-item>

        <el-form-item>
            <el-button type="primary" @click="submitForm(ruleFormRef)">
                提交
            </el-button>
            <!--  <el-button @click="resetForm(ruleFormRef)">重置</el-button> -->
        </el-form-item>
    </el-form>
</template>

<script lang="ts" setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { datas } from '@/ts/brandName'
import { FormInstance, FormRules, ElMessage } from 'element-plus'
import { Plus } from '@element-plus/icons-vue'
import { AddCar as AddCarAPI, UploadFiles as UploadFileAPI } from '@/ts/RequestInterface';

const brand_names = datas;
const ruleFormRef = ref<FormInstance>()
const router=useRouter();

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
    DownPayment: ''//首付
})
function clickBrands(str: string) {
    inputCar.BrandName = str;
}
//规则
const rules = reactive<FormRules<typeof inputCar>>({
    BrandName: [
        { required: true, message: '必填', trigger: 'blur' },
        { min: 2, max: 15, message: 'Length should be 2 to 15', trigger: 'blur' },
    ],
    Name: [
        { required: true, message: '必填', trigger: 'blur' },
        { min: 2, max: 15, message: 'Length should be 2 to 15', trigger: 'blur' },
    ],
    VehicleSource: [
        { required: true, message: '必填', trigger: 'blur' },
    ],
    VehicleColor: [
        { required: true, message: '必填', trigger: 'blur' },
    ],
    vehicleAge: [
        { required: true, message: '必填' },
        { type: 'number', message: 'This must be a number' },
    ],
    TransfersNumber: [
        { required: true, message: '必填' },
        { type: 'number', message: 'This must be a number' },
    ],
    Mileage: [
        { required: true, message: '必填' },
        { type: 'number', message: 'This must be a number' },
    ],
    SellingPrice: [
        { required: true, message: '必填' },

    ],
    TransmissionType: [
        {
            required: true, message: '必填', trigger: 'change',
        },
    ],
    Fuel: [
        {
            required: true, message: '必填', trigger: 'change',
        },
    ],
    Seating: [
        {
            required: true, message: '必填', trigger: 'change',
        },
    ],
    RegistrationTime: [
        {
            required: true,
            message: '必填',
            trigger: 'change',
        },
    ],
    Registration: [{
        required: true, message: '请上传行驶证', trigger: 'blur'
    }],
    Appearance: [{
        required: true, message: '请上传外观图片', trigger: 'blur'
    }],
    VehiTrim: [{
        required: true, message: '请上传内饰图片', trigger: 'blur'
    }],
    VehicleCondition: [
        { required: true, message: '请填写车况信息', trigger: 'blur' },
    ],
})



const options = ref(Array.from({ length: 6 }).map((_, idx) => ({
    value: `${idx + 1}`,
    label: `${idx + 1}`,
})))
options.value[6]={ value: "七座及以上", label: "七座及以上" }

const disabledDate = (time: Date) => {
    return time.getTime() > Date.now()
}
const RFileList = ref([]); // 行驶证图片列表
const AFileList = ref([]); // 外观图片列表
const VFileList = ref([]); // 外观图片列表

const dialogImageUrl = ref(""); // 预览图url
const dialogVisible = ref(false); // 预览弹窗

const RhideUpload = ref(false); // 是否隐藏上传按钮
const AhideUpload = ref(false); // 是否隐藏上传按钮
const VhideUpload = ref(false); // 是否隐藏上传按钮

// 更新上传加号按钮显示状态
function RupdateUploadShown() {
    //console.log(RFileList.value.length);

    if (RFileList.value.length == 1) {
        RhideUpload.value = true;
    } else {
        RhideUpload.value = false;
    }

    inputCar.Registration = "1";
}
function AupdateUploadShown() {
    if (AFileList.value.length == 20) {
        AhideUpload.value = true;
    } else {
        AhideUpload.value = false;
    }
    inputCar.Appearance = "1";
}
function VupdateUploadShown() {
    //console.log(VFileList.value.length);

    if (VFileList.value.length == 20) {
        VhideUpload.value = true;
    } else {
        VhideUpload.value = false;
    }
    inputCar.VehiTrim = "1";
}
// 文件改变
function RfileChange(file: any, resfileList: any) {
    RFileList.value = resfileList;
    RupdateUploadShown();
    inputCar.Registration = resfileList;
}
function AfileChange(file: any, resfileList: any) {
    AFileList.value = resfileList;
    AupdateUploadShown();
}
function VfileChange(file: any, resfileList: any) {
    VFileList.value = resfileList;
    VupdateUploadShown();
}
// 移除图片
const RhandleRemove = (file: any) => {
    const list = RFileList.value;
    for (const i in list) {
        if (list[i].uid === file.uid) {
            list.splice(i, 1);
        }
    }
    RFileList.value = list;
    RupdateUploadShown();
};
const AhandleRemove = (file: any) => {
    const list = AFileList.value;
    for (const i in list) {
        if (list[i].uid === file.uid) {
            list.splice(i, 1);
        }
    }
    AFileList.value = list;
    AupdateUploadShown();
};
const VhandleRemove = (file: any) => {
    const list = VFileList.value;
    for (const i in list) {
        if (list[i].uid === file.uid) {
            list.splice(i, 1);
        }
    }
    VFileList.value = list;
    VupdateUploadShown();
};
// 预览图片
const handlePictureCardPreview = (file: any) => {
    dialogImageUrl.value = file.url;
    dialogVisible.value = true;
};

//日期处理，将中国标准时间转换成标准时间 yyyy-mm-dd 格式
const formatDate = (time: any) => {
    const date = new Date(time);
    const year = date.getFullYear();
    const m1 = date.getMonth() + 1;
    const month = m1 < 10 ? `0${m1}` : m1;
    const d = date.getDate();
    const day = d < 10 ? `0${d}` : d;
    return `${year}-${month}-${day}`;
}
//重写上传行驶证
async function VehicleConditionFile() {
    inputCar.Registration = '';
    let param = new FormData();
    for (var i = 0; i < RFileList._rawValue.length; i++) {
        param.append('file', RFileList._rawValue[i].raw);
    }
    param.append('type', "r");
    await UploadFileAPI(param).then(function (response: any) {

        inputCar.Registration += response
    }).catch(function (error: any) {
        console.log(error.response);
    });
}
//重写上传外观图片
async function AppearanceFile() {
    inputCar.Appearance = '';
    let param = new FormData();
    for (var i = 0; i < AFileList._rawValue.length; i++) {
        param.append('file', AFileList._rawValue[i].raw);
    }
    param.append('type', "a");
    await UploadFileAPI(param).then(function (response: any) {

        inputCar.Appearance += response

    }).catch(function (error: any) {
        console.log(error.response);
    });
}

//重写上传内饰图片
async function VehiTrimFile() {
    inputCar.VehiTrim = '';
    let param = new FormData();
    for (var i = 0; i < VFileList._rawValue.length; i++) {
        param.append('file', VFileList._rawValue[i].raw);
    }
    param.append('type', "t");
    await UploadFileAPI(param).then(function (response: any) {
        //console.log("3.内饰");
        inputCar.VehiTrim += response
    }).catch(function (error: any) {
        console.log(error.response);
    });
}

const Emits = defineEmits(["refresh"]);
const submit = async () => {
    await VehicleConditionFile();
    await AppearanceFile();
    await VehiTrimFile();

    inputCar.RegistrationTime = await formatDate(inputCar.RegistrationTime);

    await AddCarAPI(inputCar)
        .then(function (response: any) {
            if (response) {
                //console.log(response);
                ElMessage({
                    message: '提交成功，请耐心等待审核',
                    type: 'seccess'
                })
                Emits("refresh")
                //router.replace({name:'CarInfoList'})
            }
            else{
                ElMessage.error("添加失败")
            }

            //router.replace({name:'test1'})

        }).catch(function (error: any) {
            console.log("错误");
            console.log(error.response);
        });
}
//提交表单
const submitForm = async (formEl: FormInstance | undefined) => {
    if (!formEl) return
    await formEl.validate((valid, fields) => {
        if (valid) {
            submit();
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
.w-50 {
    width: 20rem
}

.m-2 {
    margin: 1rem
}

.brands {
    width: 915px;
    height: 500px;
    position: absolute;
    display: none;
    background-color: white;
    overflow: auto;
    top: 97%;
    box-shadow: 0 -1px 5px 0 rgba(0, 0, 0, .1);
    z-index: 4;
    border-radius: 1%;
    padding-left: 10px;
    padding-bottom: 10px;
}

.allBrands:hover .brands {
    display: block;
}

.capital {
    margin-top: 10px;
}

.brandName {
    margin: 0 5px;
}

.group a {
    cursor: pointer;
}

.brandName:hover {
    color: red;
}
</style>