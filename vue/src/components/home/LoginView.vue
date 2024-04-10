<template>
    <div>
        <element class="login">
            <div @click="imgClose">
                <img src="../../assets/imgClose.png"  class="imgClose" alt="Close">
            </div>
            <div class="login-title">{{ LoginMethods }}</div>
            <div class="login-wrapper">

                <div class="login-dialog">
                    <el-form action=""  ref="ruleFormRef" :model="FormData">
                        <table class="login-table">
                            <tr>
                                <td>
                                    <el-form-item prop="phoneInput" required >
                                        <el-input placeholder="请输入手机号" v-model="FormData.phoneInput" clearable  />
                                    </el-form-item>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <el-form-item v-if="way == 1" prop="passwordInput" required >
                                        <el-input placeholder="请输入密码" v-model="FormData.passwordInput" show-password
                                            type="password" />
                                    </el-form-item>
                                    <el-form-item v-else>
                                        <el-input placeholder="请输入验证码" v-model="FormData.verification" clearable />
                                    </el-form-item>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <el-button  v-if="way == 1" @click="loginByPassWord" type="primary" round>登录
                                    </el-button>
                                    <el-button v-else @click="loginByCode" type="primary" round>登录或注册
                                    </el-button>
                                    <el-button v-if="way == 1" @click="switchover" type="primary" round>忘记密码
                                    </el-button>
                                    <el-button v-else @click="GetCode" type="primary" round>获取验证码
                                    </el-button>
                                </td>
                            </tr>
                        </table>
                    </el-form>
                </div>
                <div class="login_other_method">
                    <span class="login-other-label">
                        其他方式：
                    </span>
                    <div class="login-other-logo"><img src="../../assets/douyin..png" alt="抖音"></div>
                    <div class="login-other-logo"><img src="../../assets/weixin.png" alt="微信"></div>
                    <span class="login-swith" @click="switchover">{{ way1 }}</span>
                </div>
            </div>

        </element>
        <div class="shield">
        </div>
    </div>
</template>
<script lang="ts" setup>

import { login as loginApi,GetCode as GetCodeApi,LoginByCode as LoginByCodeAPI } from '@/ts/RequestInterface';

import {  reactive, ref } from 'vue';

import type { FormInstance } from 'element-plus'
import { ElMessage } from 'element-plus'

let FormData = reactive({ phoneInput: '15077490574', passwordInput: '123456', verification: "", })
let LoginMethods = ref("密码登录")
let way = ref(1);
let way1 = ref("验证码登录")

const Emits = defineEmits(["LoginOut", "login2"]);

const ruleFormRef = ref<FormInstance>()

/* const validatePhone = (rule: any, value: any, callback: any) => {
    if (value === '') {
        callback(new Error('Please input the Phone'))
    }
    callback()

}
const validatePass = (rule: any, value: any, callback: any) => {
    if (value === '') {
        callback(new Error('Please input the password'))
    }
    callback()

}
const rules = reactive<FormRules<typeof FormData>>({
    phoneInput: [{ validator: validatePhone, trigger: 'blur' }],
    passwordInput: [{ validator: validatePass, trigger: 'blur' }],

}) */

function imgClose() {
    Emits("LoginOut");
}
function loginByPassWord() {
    loginApi(FormData.phoneInput, FormData.passwordInput).then(function (response: any) {
        //console.log(response);
        
        if (response.data == true) {
            ElMessage({
                message: '登录成功',
                type: 'success'
            })
            Emits("login2");
            sessionStorage.setItem('Cartoken', response.tokenString);
            //localStorage.setItem('Cartoken', response.tokenString);
        }
        else {
            ElMessage({
                message: response.msg,
                type: 'warning'
            })
        }
    }).catch(function (error: any) {
        console.log(error);
        //router.push({ name: 'UserInfo' })
    });


    /* axios.post('/UserLogin/CheckPwd', {
        phone: FormData.phoneInput,
        password: FormData.passwordInput
    })
        .then(function (response: any) {
            if(response.status>=200 && response.status<300){
                if (response.data == "ok") {
                    Emits("login2");
                }
            }
            else{
                console.log("响应错误");
            }            
        })
        .catch(function (error: any) {
            console.log(error.response.data);
            //router.push({ name: 'UserInfo' })
        }); */
}
function switchover() {
    if (way.value == 1) {
        way.value = 2;
        way1.value = "密码登录";
        LoginMethods.value = "验证码登录";
    }
    else {
        way.value = 1;
        way1.value = "验证码登录";
        LoginMethods.value = "密码登录";
    }

}
const GetCode = async ()=>{
    var data= await GetCodeApi(FormData.phoneInput);
    console.log(data);
    if(data.result){
        ElMessage({
                message: data.msg,
                type: 'success'
            })
    }
    else{
        ElMessage({
                message:  data.msg,
                type: 'warning'
            })
    }
}
const loginByCode=async ()=>{
        if(FormData.verification==""){
             ElMessage({
                message:  "请输入验证码",
                type: 'warning'
            })
        }
        else if(FormData.phoneInput==""){
             ElMessage({
                message:  "请输入手机号",
                type: 'warning'
            })
        }
        else{
            LoginByCodeAPI(FormData.phoneInput, FormData.verification).then(function (response: any) {
        //console.log(response);
        
        if (response.result == true) {
            ElMessage({
                message: '登录成功',
                type: 'success'
            })
            Emits("login2");
            sessionStorage.setItem('Cartoken', response.tokenString);
            //localStorage.setItem('Cartoken', response.tokenString);
        }
        else {
            ElMessage({
                message: response.msg,
                type: 'warning'
            })
        }
    }).catch(function (error: any) {
        console.log(error);
        //router.push({ name: 'UserInfo' })
    });
        }
}
</script>
<style scoped>
.shield {
    position: fixed;
    width: 100%;
    height: 100%;
    z-index: 1;
    opacity: 0.5;
    background-color: rgb(85, 77, 77);
    top: 0%;
    left: 0%;
}

.imgClose {
    width: 30px;
    height: 30px;
    float: right;
    margin: 10px;
    cursor: pointer;
}

.login-table {
    width: 225px;
    text-align: center;
    height: 165px;
}

.login-wrapper {
    position: absolute;
    left: 13%;
}

.login {
    position: fixed;
    top: 30%;
    right: 35%;
    width: 400px;
    height: 300px;
    z-index: 2;
    background-color: rgb(255, 255, 255);
}

.login-dialog {
    padding: 20px 25px;
    text-align: center;
}

.login-title {
    padding: 20px 0px 0px 20px;
    line-height: 24px;
    font-size: 20px;
    font-weight: 500;
}

.el-button.is-round {
    width: 45%;
}

.login_other_method {
    height: 20px;
}

.login-other-logo {
    float: left;
    width: 20px;
    height: 20px;
    margin-right: 4px;
    cursor: pointer;
}

img,
video {
    max-width: 100%;
}

.login-other-label {
    float: left;
    margin-left: 25px;
}

.login-swith {
    float: right;
    margin-right: 25px;
    cursor: pointer;
}

.login_other_method span {
    color: rgb(151, 154, 168);
    size: 14px;
}
</style>