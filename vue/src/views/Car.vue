<template>
    <div class="bigDiv" v-loading="loading">
        <div class="topDiv">
            <div class="topLeftDiv">
                <div style="width: 100%;height: 85%;">
                    <img :src="imageUrl" alt="">
                </div>
                <div class="leftBottomDiv">
                    <div class="leftBottomBigDiv">
                        <div v-for="i in picture" class="leftBottomSmallDiv" @click="pictureClick(i)">
                            <img :src="i" alt="">
                        </div>
                    </div>
                    <!--  -->
                </div>
            </div>
            <div class="topRightDiv">
                <div style="height: 30px;">
                    <h1 style="font-size: 20px;font-weight: 600;">
                        <span>
                            {{ data.name }}
                        </span>
                    </h1>
                </div>
                <div style="height: 30px;">
                    <p
                        style="padding-left: 4px;padding-right: 4px;margin-bottom: 6px ;background-color: rgba(242,244,250);width: 70px;">
                        含过户费
                    </p>
                </div>
                <div class="price" style="margin-top: 10px;">
                    <span
                        style="--tw-text-opacity: 1;color: rgba(230,32,33,var(--tw-text-opacity));font-size: 20px;font-weight: 600;">
                        {{ data.sellingPrice }}万
                    </span>

                </div>
                <div style="height: 70px;width: 50%;margin-top: 17px;display: flex;cursor: pointer;">
                    <span>
                        <el-icon size="65px" style="color: #0B9AED;">
                            <ShoppingBag />
                        </el-icon>
                    </span>
                    <span style="line-height: 70px;">首付{{ data.downPayment }}万开回家</span>
                    <span style="line-height: 70px;margin-left: 10px;">
                        分期购
                    </span>
                </div>
                <div style="width: 100%;height:90px;display: flex;margin-top: 20px;text-align: center;">
                    <div class="details">
                        <h1> {{ data.mileage }} 万公里</h1>
                        <h2>里程</h2>
                    </div>
                    <div class="details">
                        <h1>{{ data.vehicleSource }}</h1>
                        <h2>车源地</h2>
                    </div>
                    <div class="details">
                        <h1>{{ data.transfersNumber }}次</h1>
                        <h2>过户次数</h2>
                    </div>
                    <div class="details">
                        <h1>{{ data.registrationTime }}</h1>
                        <h2>上牌时间</h2>
                    </div>
                </div>
                <div style="height: 52px;width: 100%;display: flex;text-align: center;line-height: 50px;">
                    <div style="width: calc(40%);height: 100%;background-color: chartreuse;margin-right: 15px;cursor: pointer;"
                        @click="contactNumber">
                        <span>
                            <el-icon>
                                <Phone />
                            </el-icon>
                        </span>
                        <span>点击查看联系电话</span>
                    </div>
                    <div style="width: calc(20%);height: 100%;background-color: chartreuse;cursor: pointer;"
                        @click="BuyCar"> <span>
                            立即购车
                        </span>
                        <span>
                            <el-icon size="20px">
                                <ShoppingCartFull />
                            </el-icon>
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <div class="carInfo">
            <div>
                <h3>
                    车辆档案
                </h3>
            </div>
            <div style="display: flex;height: 75px;text-align: center;">
                <div v-for="i in 9" style="width: calc(100%/9); background-color: #0B9AED;margin: 3px;padding-top: 1%;">
                    {{ i }}
                    <div>
                        <span>
                            燃油类型
                        </span>
                    </div>
                </div>
            </div>
            <div>
                <h3>
                    车况介绍
                </h3>
                <span>
                    {{ data.vehicleCondition }}
                    <!-- DasWeltAuto一汽-大众官方认证二手车[厂家二手车全国一汽-大众4S店联保]专业检测评估结果：该车经过一汽-大众133项严格检测，涵盖静态检测、动态监测、仪器设备及底盘检测等，经过评级，达到厂家标准，可享受一汽-大众一年或20000公里全国联保，同时满足国标GB/T30323-2013标准，具有认证二手车品牌优势。该车通过厂家，安全、性能、外观和内饰全部达到一汽-大众检测标准A级评级，是难得的精品好车，在任意一家一汽-大众4S店均可享受经销商保修服务！品牌服务桂林鑫广达汽车销售服务有限责任公司为DasWeltAuto一汽-大众品质二手车授权经销商，业务涵盖二手车评估、收购、销售、置换、代办过户，并为客户提供整车质量保证、24小时道路救援、车辆美容翻新等经销商服务。我们所有认证二手车均经过一汽-大众厂家评估师的133项全方位检测，杜绝事故车、火烧车水泡车，保证车辆品质。本店为客户提供分期购车服务！欢迎到店看车！试车！ -->
                </span>
            </div>
        </div>
        <div>
            <div>
                <h3>
                    车辆图片
                </h3>
                <div>
                    <div v-for="i in picture" style="width: calc(48.2%); margin:10px; float: left;">
                        <img :src="i" alt="">
                    </div>
                </div>

            </div>
        </div>
        <!--  汽车详情界面{{ data.Amortize }}{{ data.BrandName }} -->
    </div>
</template>

<script lang="ts" setup>

import { reactive, ref, onMounted, h } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import type { Action } from 'element-plus'
import { ElMessage, ElMessageBox } from 'element-plus'
import { baseURL, GetCarInfo as GetCarInfoApi, GetNamePhoneByCarId as GetNamePhoneApi } from '@/ts/RequestInterface';
import Qs from 'qs'

const router = useRouter();

const loading = ref(true)
let data = reactive({
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
    VehicleLevelName: '',//汽车类型
    Registration: '',//行驶证
    appearance: '',//外观
    vehiTrim: '',
    vehicleCondition: '',//描述
    sellingPrice: '',//售价
    Amortize: false,//是否分期
    downPayment: ''//首付
})
let imageUrl = ref(require("@/assets/2.jpg"));

let picture: string[] = reactive([]);
const id = useRoute().params.Number;
onMounted(async () => { // 在组件被创建时调用加载数据的方法

    const datas = { Id: id, tf: true }
    data = (await GetCarInfoApi(Qs.stringify(datas))).data;
    if (data.name == null) {
        for (var i = 0; i < 3; i++) {
            ElMessage({
                message: '警告，非法查询',
                type: 'warning'
            });
        }
        router.push({ name: 'HomePage' })
    }
    else {
        let arr = data.appearance.split(";");
        arr.forEach((item: string) => {
            if (item != "") {
                picture.push(baseURL + item)
            }
        })
        arr = data.vehiTrim.split(";");
        arr.forEach((item: string) => {
            if (item != "") {

                picture.push(baseURL + item)
            }
        })
        imageUrl.value = picture[0];
        //console.log(data);
        loading.value = false;
    }


});

const pictureClick = (pictureurl: any) => {
    imageUrl.value = pictureurl;
}
const BuyCar = () => {
    let ob = JSON.stringify({ id: id })
    router.push({ name: 'SalesTicket', state: { ob } })
}

const contactNumber = async () => {
    await GetNamePhoneApi(Number(id)).then(function (Response: any) {

        ElMessageBox.alert(Response.name + '，' + Response.phoneNumber, '联系方式', {
            confirmButtonText: 'OK',
            callback: (action: Action) => {
            },
        });
    }).catch(function (error: any) {

    })


}

</script>

<style scoped>
.bigDiv {
    margin: 0 auto;
    width: 93%;

    box-shadow: 0 -1px 5px 0 rgba(0, 0, 0, .1);
    padding-left: 5px;
}

.topDiv {
    height: 380px;
    width: 100%;
    /* background-color: rgb(15, 115, 187); */
}

.topLeftDiv {
    height: 100%;
    width: 40%;
    /* margin: 10px 0px 0px 10px; */
    /* background-color: bisque; */
    float: left;
}

img {
    width: 100%;
    height: 100%;
}

.leftBottomDiv {
    width: 100%;
    height: 15%;
    /* background-color: blueviolet; */
    /*  display: flex; */
    overflow: hidden;
}

.topRightDiv {
    height: 85%;
    width: 55%;
    /*  background-color: aquamarine; */
    margin-left: 2%;
    padding-left: 20px;
    float: left;
}

img {
    width: 100%;
    height: 100%;
}

.leftBottomBigDiv {
    display: flex;
    height: 100%;
    width: 800px;
    background-color: bisque;
    transform: translate3d(0%, 0px, 0px)
}

.leftBottomSmallDiv {
    width: calc(100% / 6);
    height: 94%;
    /* background-color: chocolate; */
    margin: 2px;
    cursor: pointer;

}

.details {
    background-color: rgb(247, 248, 252);
    width: calc(100%/4);
    margin: 5px;
}

.details h1 {
    font-size: 18px;
}

.details h2 {
    font-size: 14px;
}
</style>