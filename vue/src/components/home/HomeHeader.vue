<template>
    <div>
        <Login v-if="LoginType == 1" @LoginOut="LoginOut" @Login2="login2" />
     
    <div class="header">
        <div class="sign-city">
            <a href="">
                <img class="sign" src="@/assets/log1.png" alt="">
            </a>
            <div class="header-city">
                <span>
                    <el-icon>
                        <caret-bottom></caret-bottom>
                    </el-icon>
                </span>
                <span class="header-city-name">{{ CityName }}</span>
                <div class="city_show">
                    <City @choiceCity="choiceCity" />
                </div>
            </div>
        </div>
        <div class="navlist">
            <a :class="{a_line:checked== 1 }" @click="shouye">首页</a>
            
            <a :class="{a_line:checked== 2 }"  @click="buy('','')">我要买车</a>
            
            <a :class="{a_line:checked== 3 }"  @click="sell">我要卖车</a>
        </div>
        <div class="login">
            <div class="header-phone">
                热线电话 0773-2230110
            </div>
            <el-dropdown  v-if="LoginType == 2">
                <button class="loginBt">欢迎</button> 
                <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item @click="UserCenter">个人中心</el-dropdown-item>
                <el-dropdown-item @click="LoginOut">退出登录</el-dropdown-item>
              </el-dropdown-menu>
            </template>
            </el-dropdown>
            <button  v-else class="loginBt" @click="Login1">登录</button>

        </div>
    </div>
</div> 
</template>

<script lang="ts" setup>
import Login from './LoginView.vue'
import City from './CityView.vue'

import mitss from '@/ts/mitt'
import { useRouter,useRoute  } from 'vue-router'
import { ref,onBeforeMount,onMounted,watch } from 'vue';

/* const route=useRoute();
const lt=route.params.LoginType;
console.log(lt); */


let LoginType = ref(0);
onBeforeMount(()=>{
    mitss.on('PriceConditions',(e:string)=>{
        buy("",e,);
    })
    mitss.on('BrandConditions',(e:string)=>{
        buy(e,"");
    })
})

onMounted(()=>{
    const carToken=sessionStorage.getItem('Cartoken');
    if(carToken){
        LoginType.value=2
    }
    mitss.emit('DDD', CityName.value)
})

//LoginType.value=Number(useRoute().params.LoginType);

let checked = ref(1)
let CityName = ref("桂林市");
const router=useRouter();
function Login1() {
    LoginType.value = 1;
} 
const login2=()=>{
    LoginType.value = 2;
}
/* function login2() {
    LoginType.value = 2;
}  */
function LoginOut() {
    LoginType.value = 0;
    sessionStorage.removeItem('Cartoken');
    localStorage.removeItem('Cartoken');
} 
function shouye() {
    checked.value = 1;
    router.replace({name:'HomePage'});
} 
function buy(Condition1:string,Condition2:string) {
    let ob = JSON.stringify({ cname: CityName.value,BrandsCondition: Condition1,PriceCondition:Condition2})
    router.replace({name:'Buy',state: { ob } });
    /* if(num==0){
        
    }
    if(num==1){
        router.replace({name:'Buy', params: { BrandsCondition: "", PriceCondition: Condition } });
    }
    if(num==2){
        router.replace({name:'Buy', params: { BrandsCondition:Condition, PriceCondition: "" } });
    } */
    checked.value = 2;
} 
function sell() {
    checked.value = 3;
    router.replace({ name: 'ConsignmentOrder' })
}
function choiceCity(name: string) {
    CityName.value = name;
}
function UserCenter(){
    router.push({ name: 'UserInfo'})
}
/* watch(CityName,(newValues, oldValues)=>{
    
}) */
</script>

<style scoped>
.login {
    /*  width: 270px; */
    /*height: 20px; */
    float: right;
    margin-top: 1%;
    margin-right: 3%;
}

.loginBt {
    width: 60px;
    height: 36px;
    border-radius: 5px;
    background-color: #ffcc32;
    cursor: pointer;
    font-weight: 500;
    border: none;
    float: right;
}

.header-phone {
    width: 190px;
    /* height: 40px; */
    margin-top: 2%;
    float: left;
}

.sign-city {
    float: left;
    height: 100%;
    width: 260px;
    margin-left: 3%;
}

.sign {
    border-radius: 50%;
}

.header-city {
    cursor: pointer;
    float: right;
    font-weight: 500;
    color: #181818;
    height: 67%;
    width: 76%;
    padding-top: 9%
}

.header-city span {
    float: right;
}

.city_show {
    position: absolute;
    display: none;
    margin-top: 2.9%;
    z-index: 1;
}

.header-city-name {
    height: 40px;
}

.header-city:hover .header-city-name {
    color: red;
}

.header-city:hover .city_show {
    display: block;
}

.navlist {
    float: left;
    height: 100%;
    width: 28%;
    margin-left: 5%;
    /*  background-color: #ffcc32; */
    text-align: center;
}

.navlist a {
    line-height: 65px;
    margin: 0 12px 10px 50px;
    padding-bottom: 15px;
    text-decoration: none;
    cursor: pointer;
}

.a_line {
    border-bottom: 2px solid #302a2aad;
}</style>
