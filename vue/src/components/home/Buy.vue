<template>
    <div v-loading="loading">
        <div class="city">
            <span>
                {{ brandName }}二手车
            </span>
            <el-button type="primary" @click="search">搜索</el-button>
            <div class="searchBox">
                <el-input v-model="input1" class="w-50 m-2" size="large" placeholder="关键词" :clearable="true" />
            </div>
        </div>
        <div class="condition">
            <div class="box">

                <div class="left_box">
                    <span>品牌</span>
                </div>
                <div class="right_box">
                    <a class="right_box_a" @click="brandbuxian">
                        <span>不限</span>
                    </a>
                    <a class="right_box_a" v-for="(item, index) in brand_name" :key="index" @click="chooseBrand(item)">
                        <span>{{ item }}</span>
                    </a>
                    <div class="all">
                        <a class="right_box_a" @click="brandShow">
                            <span v-if="brandsShow">收起<el-icon>
                                    <ArrowUpBold />
                                </el-icon></span>
                            <span v-else>全部<el-icon>
                                    <ArrowDownBold />
                                </el-icon></span>
                        </a>
                    </div>
                    <div class="brands" v-if="brandsShow">
                        <div class="group" v-for="(intem, index) in brand_names" :key="index">
                            <p class="capital" style="color:red"> {{ intem.letter }}</p>
                            <a class="brandName" v-for="(carName, index) in intem.name" :key="index"
                                @click="chooseBrand(carName)">
                                <span>{{ carName }} </span>

                            </a>
                        </div>

                    </div>

                </div>
            </div>
            <div class="box">
                <div class="left_box">
                    <span>价格</span>
                </div>
                <div class="right_box">
                    <a class="right_box_a" @click="pricebuxian">
                        <span>不限</span>
                    </a>
                    <a class="right_box_a" v-for="(item, index) in prices" :key="index" @click="price(item)">
                        <span>{{ item }}</span>
                    </a>
                    <input type="text" class="price-input" v-model="low_price">
                    -
                    <input type="text" class="price-input" v-model="height_price">
                    <span class="unit">万</span>
                    <span class="price_btn" @click="price_btn">确定</span>
                </div>
            </div>
            <div class="box">
                <div class="left_box">
                    <span>更多</span>
                </div>
                <div class="right_box">
                    <el-select v-model="vehicle_age_value" placeholder="车龄" clearable class="right_box_select">
                        <el-option v-for="item in vehicle_age" :key="item.value" :label="item.label"
                            :value="item.value" />
                    </el-select>
                    <el-select v-model="mileage_value" placeholder="里程" clearable class="right_box_select">
                        <el-option v-for="item in mileage" :key="item.value" :label="item.label" :value="item.value" />
                    </el-select>
                    <el-select v-model="Transmission_type_value" placeholder="变速箱" clearable class="right_box_select">
                        <el-option v-for="item in Transmission_type" :key="item.value" :label="item.label"
                            :value="item.value" />
                    </el-select>
                    <el-select v-model="fuel_type_value" placeholder="燃油类型" clearable class="right_box_select">
                        <el-option v-for="item in fuel_type" :key="item.value" :label="item.label"
                            :value="item.value" />
                    </el-select>
                    <el-select v-model="seating_value" placeholder="座位数" clearable class="right_box_select">
                        <el-option v-for="item in seating" :key="item.value" :label="item.label" :value="item.value" />
                    </el-select>
                </div>
            </div>
            <div class="currentConditions">
                <span>当前筛选：</span>
                <!--  <template v-for="(item, index) in condition" :key="index">
                <span v-if="item.value != '' && item.value != undefined" style="margin-right:10px">
                    {{ item.key }}:{{ item.value }}
                    
                </span>
                <span  v-if="item.value != undefined">
                    {{ test1(index,item.value) }}
                </span>
            </template> -->
                <span v-if="brandName != ''" style="margin-right:10px">
                    品牌：{{ brandName }}
                </span>
                <span v-if="pricess != ''" style="margin-right:10px">
                    价格：{{ pricess }}
                </span><span v-if="vehicle_age_value != ''" style="margin-right:10px">
                    车龄：
                    <span v-if="vehicle_age_value != '9'">{{ vehicle_age_value }}年以内</span>
                    <span v-else>8年及以上</span>
                </span><span v-if="mileage_value != ''" style="margin-right:10px">
                    里程数：
                    <span v-if="mileage_value!='11'">{{ mileage_value }}万公里以内</span>
                    <span v-else>10万公里以上</span>
                </span><span v-if="Transmission_type_value != ''" style="margin-right:10px">
                    变速箱：{{ Transmission_type_value }}
                </span><span v-if="fuel_type_value != ''" style="margin-right:10px">
                    燃油类型：{{ fuel_type_value }}
                </span><span v-if="seating_value != ''" style="margin-right:10px">
                    座位数：{{ seating_value }}座
                </span>
            </div>
        </div>
        <div class="navigation">
            <div class="cars" v-for="(item, index) in dtList" :key="index" @click="car(item.id)">
                <div class="img">
                    <img :src="baseURL + item.appearance" alt="">
                </div>
                <div class="introduce">
                    <span>{{ item.name }}</span>
                </div>
                <div class="Year_Kilometers">
                    {{ item.vehicleAge }}年<span class="tags-split">|</span>
                    <span> {{ item.mileage }}万公里</span>
                </div>
                <div class="SellingPrice_DownPayment">
                    <span class="SellingPrice">{{ item.sellingPrice
                        }} </span>
                    <span class="unit">万</span>
                    <span class="DownPayment"> 首付{{ item.downPayment
                        }}万</span>
                </div>
            </div>
        </div>
        <div style="width: 100%;float: left;margin-top: 1%;" >
        <div class="el-pagination" v-if="tatleCount!=0">
            <el-pagination layout="prev, pager, next" :total="tatleCount"  @current-change="currentChange" v-model:current-page="currentPage"/>
        </div>
    </div>
    </div>


</template>

<script lang="ts" setup>

import { datas } from '../../ts/brandName'
import { ref, reactive, onBeforeMount, onMounted, watch, toRaw } from 'vue'
import { useRoute,useRouter } from 'vue-router'

import { baseURL, GetSearchCar as GetSearchCarApi } from '@/ts/RequestInterface'
import mitss from '@/ts/mitt'
import { ElMessage } from 'element-plus'
//const Emits = defineEmits(["choiceCity"]);


const route = useRoute();



let brand_names = datas
let brand_name = ["大众", "别克", "丰田", "本田", "宝马", "奥迪", "吉利", "红旗", "名爵"]
let prices = ["2万以下", "2万~4万", "4万~6万", "6万~8万", "8万~10万"]
let input1 = ref("")
let brandsShow = ref(false)
let brandName = ref("")
let pricess = ref("")
let height_price = ref("")
let low_price = ref("")
let mileage_value = ref("")
let vehicle_age_value = ref("")
let Transmission_type_value = ref("")
let fuel_type_value = ref("")
let seating_value = ref("")
let vehicle_age = reactive([
    {
        value: '1',
        label: '1年以内',
    },
    {
        value: '3',
        label: '3年以内',
    },
    {
        value: '5',
        label: '5年以内',
    },
    {
        value: '8',
        label: '8年以内',
    },
    {
        value: '9',
        label: '8年及以上',
    },
])
const mileage = reactive([
    {
        value: '1',
        label: '一万公里以内',
    },
    {
        value: '3',
        label: '三万公里以内',
    },
    {
        value: '5',
        label: '五万公里以内',
    },
    {
        value: '8',
        label: '八万公里以内',
    },
    {
        value: '10',
        label: '十万公里以内',
    },
    {
        value: '11',
        label: '十万公里以上',
    }
])
const Transmission_type = reactive([
    {
        value: '手动',
        label: '手动',
    }, {
        value: '自动',
        label: '自动',
    }
])
const fuel_type = reactive([
    {
        value: '汽油',
        label: '汽油',
    }, {
        value: '电动',
        label: '电动',
    }, {
        value: '油电混动',
        label: '油电混动',
    }
])
const seating = reactive([
    {
        value: '2',
        label: '2座',
    }, {
        value: '4',
        label: '4座',
    }, {
        value: '5',
        label: '5座',
    }, {
        value: '6',
        label: '6座',
    }, {
        value: '7',
        label: '7座及以上',
    }
])




const CurrenCd = ref("")

function search() {
    data.Name=input1.value;
    qq(1,12,toRaw(data));
}
function brandShow() {
    brandsShow.value = (!brandsShow.value);
}
function chooseBrand(str: string) {
    brandName.value = str;
}
function price_btn() {
    if (height_price.value == '') {

        alert("请输入最高价");
    }
    else if (low_price.value == '') {
        pricess.value = "0~" + height_price.value + "万";
    }
    else {
        pricess.value = low_price.value + "万~" + height_price.value + "万";
    }
}
function price(str: string) {
    pricess.value = str;
}
function brandbuxian() {
    brandName.value = "";
}
function pricebuxian() {
    pricess.value = ""
}
const data = reactive({
    BrandName: "",
    Name: "",
    TransmissionType: "",
    Seating: 0,
    Mileage: 0,
    SellingPrice: "",
    VehicleAge: 0,
    Fuel: "",
    VehicleSource: "",
    VehicleLevelName: ""
})
interface carInfo {
    appearance: string;
    downPayment: 0;
    mileage: 0;
    name: "";
    sellingPrice: 0;
    vehicleAge: 0;
    id: 0
}
let dtList: carInfo[] = reactive([]);

const cityName = ref("");
onBeforeMount(() => {
    mitss.on('CCC', (e: string) => {
        //console.log(e);
        cityName.value = e;
    })
})

const tatleCount=ref(0)
onMounted(() => {
    const cname = JSON.parse(history.state.ob).cname;
    const PriceCondition = JSON.parse(history.state.ob).PriceCondition;
    const BrandsCondition = JSON.parse(history.state.ob).BrandsCondition;
    cityName.value = cname;
    brandName.value = BrandsCondition;
    pricess.value = PriceCondition;
    data.BrandName=BrandsCondition;
    data.VehicleSource=cname;
    qq(1,12,toRaw(data))
})
watch([brandName, pricess, vehicle_age_value, mileage_value, Transmission_type_value, fuel_type_value, seating_value, cityName], (newValues, oldValues) => {
    CurrenCd.value = "";
    if (newValues != oldValues) {
        //console.log(brandName.value);
        //Emits("choiceCity", "city");
        data.BrandName = String(brandName.value);
        data.SellingPrice = String(pricess.value);
        data.VehicleAge = Number(vehicle_age_value.value);
        data.Mileage = Number(mileage_value.value);
        data.TransmissionType = String(Transmission_type_value.value);
        data.Fuel = String(fuel_type_value.value);
        data.Seating = Number(seating_value.value);
        data.VehicleSource = String(cityName.value);
        data.VehicleLevelName = "";
        data.Name=input1.value;
        qq(1,12,toRaw(data))

    }
})

const loading = ref(true);
const qq = async (p:number,l:number,dt: object) => {
    loading.value=true;
    //console.log(dt);
    //console.log(dtList);

    const ddd = await GetSearchCarApi(p,l,dt);
    //console.log(ddd);
    dtList = ddd.data;
    
    tatleCount.value=ddd.tatleCount;
    for (var i = 0; i < dtList.length; i++) {
        dtList[i].appearance = dtList[i].appearance.split(';')[0]
    }
    console.log(dtList);
    loading.value = false
}

const router = useRouter();
//点击跳转至汽车详情页
const car = (ID: number) => {
    const token = sessionStorage.getItem('Cartoken');
    if (token == null) {
        ElMessage({
            message: '请先登录',
            type: 'warning'
        });

    }
    else {
        const url = router.resolve({
            name: 'CarView', params: { Number: ID }
        })
        window.open(url.href, '_blank')
    }

    //router.push({  })
}

const currentPage=ref(1);
const currentChange=async (pager:number)=>{
    qq(pager,12,toRaw(data))
}
</script>
<style scoped>
.city {
    margin: 0 auto;
    width: 93%;
    height: 54px;
    /*  box-shadow: 0 -1px 5px 0 rgba(0,0,0,.1); */
    padding-left: 5px;
}

.condition {
    margin: 0 auto;
    width: 93%;
    box-shadow: 0 -1px 5px 0 rgba(0, 0, 0, .1);

}

.box {
    height: 50px;
    width: 100%;
    border-bottom: 1px solid #e6e6e6;
    text-align: center;
}

.box span {
    line-height: 50px;
}

.left_box {
    float: left;
    width: 70px;
    height: 50px;
    color: #7a838d;
    background-color: #fafafa;
}

.right_box h5 {
    font-size: 20px;
}

.right_box {
    width: 94%;
    height: 100%;
    float: left;
}

.right_box a:hover {
    color: red;
}

.right_box_a {
    cursor: pointer;
    font-size: 14px;
    display: flex;
    width: auto;
    height: 20px;
    float: left;
    margin-left: 5px;
    margin-right: 20px;
}

.right_box img {
    width: 24px;
    margin-right: 10%;
}

.right_box_select {
    color: black;
    margin: 10px 10px;
    float: left;
    width: 100px;
}

.all {
    float: right;
}

.searchBox {
    float: right;
    width: 20%;
}

.city button {
    float: right;
    margin-left: 1%;
    padding: 18px;
}

.brands {
    width: 77.3%;
    height: 500px;
    position: absolute;
    /* display: none; */
    background-color: white;
    overflow: auto;
    top: 25.4%;
    box-shadow: 0 -1px 5px 0 rgba(0, 0, 0, .1);
    z-index: 4;
    border-radius: 1%;
    padding-left: 10px;
    text-align: left;
}

.brands a {
    cursor: pointer;
}

.group {
    white-space: pre-line;
    float: left;
}

.brands a:hover {
    color: red;
}

.brandName {
    margin-top: 10px;
    margin-right: 35px;
    font-size: 14px;
    display: inline-flex;
}

.brandName span {
    line-height: 30px;
}

.price-input {
    height: 26px;
    line-height: 22px;
    padding: 0 5px;
    color: #495056;
    width: 50px;
    border: 1px solid #b7aeae;
    font-size: 12px;
    margin-top: 10px;
    /*  margin-right:2%; */
}

.unit {

    padding-left: 10px;
}

.price_btn {
    margin-right: 33%;
    padding-left: 10px;
    /*  font-size: 12px; */
    color: #22ac38;
    cursor: pointer;
}

.currentConditions {
    line-height: 50px;
    color: #a5abb2;
    padding: 8px 3px;
    line-height: 30px;
    font-size: 14px;
}

.cars {
    width: 24%;
    height: 270px;
    /* background-color: antiquewhite; */
    float: left;
    margin: 20px 5.8px;
    cursor: pointer;
}

.cars:hover {
    box-shadow: 0 -1px 5px 0 rgb(9 8 8 / 65%)
}

.img {
    width: 100%;
    height: 75%;
    /*  background-color: rgb(189, 189, 139); */
}

.img img {
    width: 100%;
    height: 100%;
}

.introduce {
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
    font-weight: bold;
    font-size: 17px;
}

.Year_Kilometers {
    color: #a5abb2;
    font-size: 14px;
    height: 22px;
    box-sizing: border-box;
    padding: 0 0 2px 4px;
    display: -webkit-box;
    display: -webkit-flex;
    display: flex;
    -webkit-box-align: center;
    -webkit-align-items: center;
    align-items: center;
}

.tags-split {
    font-size: 12px;
    width: 14px;
    text-align: center;
}

.SellingPrice_DownPayment {
    padding-left: 4px;
    height: 30px;
    font-size: 20px;
    color: #fa5428;
    overflow: hidden;
    display: -webkit-box;
    display: -webkit-flex;
    display: flex;
}

.unit {
    font-size: 14px;
    font-weight: 400;
    margin-top: 8px;
}

.DownPayment {
    font-size: 12px;
    margin-top: 8px;
    margin-left: 6px;
}

.navigation {
    width: 93%;
    height: 100%;
    /*border-style: solid;
    border-width:2px;
     border-color: rgb(73, 32, 162);
    background-color: cornflowerblue; */
    margin: 10px auto;
}
.el-pagination {
    margin:0 auto;
}
</style>