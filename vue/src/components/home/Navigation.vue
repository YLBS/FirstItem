<template>
    <div  v-loading="loading">

    
    <div class="condition" >

        <div class="brands_left">
            <h5>我要买车 <el-icon>
                    <ArrowRightBold />
                </el-icon></h5>
            <a v-for="(item, index) in brands" :key="index" @click="clickBrands(item.brandName)">
                <img :src="item.brandImg" alt="1">
                <span>{{ item.brandName }}</span>
            </a>
            <br />

            <div class="allBrands">
                <h6>全部品牌 <el-icon>
                        <CaretRight />
                    </el-icon></h6>
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
            </div>


            <a v-for="(item, index) in price" :key="index" @click="clickPrice(item)">
                <span>{{ item }}</span>
            </a>

            <br />
            <h6>更多<el-icon>
                    <CaretRight />
                </el-icon></h6>
            <a v-for="(item, index) in carType" :key="index">
                <span>{{ item }}</span>
            </a>
        </div>
        <div class="brand_right">
            <div class="searchImg">
                <img src="@/assets/search.png" alt="">
              
            </div>
            <div class="searchBox">
                <el-input v-model="input1" class="w-50 m-2" size="large" placeholder="请输入关键词，例如：君越  2.4L"
                    :clearable="true" />
            </div>
            <el-button type="primary" @click="search">搜索</el-button>
        </div>
    </div>
    <div class="navigation">

        <div>
            <h4>热卖车型</h4>
        </div>
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
        <div class="el-pagination">
            <el-pagination layout="prev, pager, next" :total="tatleCount"  @current-change="currentChange" v-model:current-page="currentPage"/>
        </div>
    </div>
</div>
</template>

<script lang="ts" setup>
import { useRouter } from 'vue-router'
import { onMounted, ref, reactive,watch,onBeforeMount } from 'vue'
import { baseURL, GetCar as GetCarApi,GetCarList as GetCarListApi } from '@/ts/RequestInterface';

import { ElMessage } from 'element-plus'
import { datas } from '@/ts/brandName'

import mitss from '@/ts/mitt'

const brands = [
    { brandName: "大众", brandImg: require("@/assets/baoma.png") }
    , { brandName: "别克", brandImg: require("@/assets/beike.png") }
    , { brandName: "丰田", brandImg: require("@/assets/fengtian.png") }
    , { brandName: "本田", brandImg: require("@/assets/baoma.png") }]
const brand_names = datas;
const input1 = ref("");
const price = ["2万以下", "2万~4万", "4万~6万", "6万~8万", "8万~10万"];
const carType = ["练手车", "SUV", "轿车", "MPV", "更多车源"];

const tatleCount=ref(0)



function clickPrice(str: string) {
    mitss.emit('PriceConditions', str)
}
function clickBrands(str: string) {
    mitss.emit('BrandConditions', str)

    //router.replace({ name: 'Buy' });
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

const loading = ref(true);
interface carInfo {
    appearance: "";
    downPayment: 0;
    mileage: 0;
    name: "";
    sellingPrice: 0;
    vehicleAge: 0;
    id: 0
}

let dtList: carInfo[] = reactive([]);
onMounted(async () => {        // 在组件被创建时调用加载数据的方法
    const response = await GetCarApi();
    dtList = response.data;
    tatleCount.value=response.tatleCount;
    loading.value = false
});
const currentPage=ref(1);

const cn=ref("桂林市")
const currentChange=async (pager:number)=>{
    loading.value=true;
    const token = sessionStorage.getItem('Cartoken');
    if (token) {
        dtList= (await GetCarListApi(pager,12,input1.value,cn.value)).data
        //console.log(dtList);
    }
    else{
        currentPage.value=1;
        ElMessage({
        message:'请先登录',
        type:'warning'
      })
    }
    loading.value = false
}
onBeforeMount(() => {
    mitss.on('DDD', (e: string) => {
        cn.value=e;       
    })
})
function search() {
    searchAction();
}
const searchAction=async ()=>{
    loading.value=true;
    var dt=await  (GetCarListApi(1,12,input1.value,cn.value))
    dtList=dt.data;
    tatleCount.value=dt.tatleCount;    
    loading.value=false;
}
watch(cn,(newValue,oldValue)=>{
    searchAction()
})
</script>

<style scoped>
.condition {
    margin: 0 auto;
    width: 93%;
    height: 300px;
    box-shadow: 0 -1px 5px 0 rgba(0, 0, 0, .1);
    padding-left: 5px;
}

.brands_left h5 {
    font-size: 20px;
}

.brands_left {
    width: 62%;
    height: 100%;
    /*  background-color: rgb(193, 103, 103); */
    float: left;
}

.brands_left a {
    cursor: pointer;
    font-size: 14px;
    display: flex;
    width: 60px;
    height: 20px;
    float: left;
    margin-right: 35px;

}

.brands_left img {
    width: 24px;
    margin-right: 10%;
}

.allBrands {
    cursor: pointer;
}

.brand_right {
    width: 37%;
    height: 100%;
    /*  background-color: thistle; */
    float: right;
}

.searchBox {
    float: left;
    width: 80%;
}

.brand_right button {
    margin-top: 1.2%;
    margin-left: 3%;
}

.searchImg {
    width: 435px;
    float: right;
}

.searchImg img {
    max-width: 100%;
}

.brands {
    width: 77.3%;
    height: 500px;
    position: absolute;
    display: none;
    background-color: white;
    overflow: auto;
    top: 31%;
    box-shadow: 0 -1px 5px 0 rgba(0, 0, 0, .1);
    z-index: 4;
    border-radius: 1%;
    padding-left: 10px;
    padding-bottom: 10px;
}

.allBrands {
    width: 70px;
    /* height:27px */
}

.allBrands h6:hover {
    color: red;
}

.allBrands:hover .brands {
    display: block;
}

.group {
    float: left;
}

.capital {
    margin-top: 10px;
}

.brandName {
    margin-top: 5px;
}

.group a {
    width: auto;
}

.brandName:hover {
    color: red;
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

.el-pagination {
    margin:0 auto;
}
</style>