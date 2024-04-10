<template>
    <div class="condition">

        <div class="brands_left">
            <h5>我要买车 <el-icon>
                    <ArrowRightBold />
                </el-icon></h5>
            <a v-for="(item,index) in brands" :key="index" @click="clickBrands(item.brandName)">
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
                        <a class="brandName" v-for="(carName, index) in intem.name" :key="index" @click="clickBrands(carName)">
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
</template>
  
<script lang="ts" setup>

//http://youlanjihua.com/youlanApi/v1/phonecode/validate.php?secret={your secret}&requestId={requestId}&code={短信验证码}
import { datas } from '../../ts/brandName'
import { useRouter } from 'vue-router'
import {getCurrentInstance } from 'vue'

import mitss from '../../ts/mitt'

const router = useRouter();

const instance = getCurrentInstance()

const brands = [
    { brandName: "大众", brandImg: require("@/assets/baoma.png") }
    , { brandName: "别克", brandImg: require("../../assets/beike.png") }
    , { brandName: "丰田", brandImg: require("../../assets/fengtian.png") }
    , { brandName: "本田", brandImg: require("../../assets/bentian.png") }
    , { brandName: "宝马", brandImg: require("../../assets/baoma.png") }]
const brand_names = datas;
const input1 = "";
const price = ["2万以下", "2万~4万", "4万~6万", "6万~8万", "8万~10万"];
const carType = ["练手车", "SUV", "轿车", "MPV", "更多车源"];



function search() {
    console.log(input1);
}
function getAssetsImages(name: string) {
    //console.log(name);
    return require('../../assets/dazong.png')
    // return new URL(`../image/${name}`, import.meta.url).href; //本地文件路径
}
function clickPrice(str: string) {
    mitss.emit('PriceConditions',str)
}
function clickBrands(str: string) {
    mitss.emit('BrandConditions',str)

    //router.replace({ name: 'Buy' });
}
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
</style>
  