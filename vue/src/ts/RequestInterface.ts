import newAxios from './newAxios'
import Qs from 'qs'

export const baseURL="http://localhost:5246/";

export const login=(phone:string,password:string)=>{
    return newAxios({
        url:"/UserLogin/CheckPwd",
        method:"post",
        data:Qs.stringify({Phone:phone,PassWord:password})
    })
}
//不需要token,只取12条数据
export const GetCar=()=>{
    return newAxios({
        url:"/Car/GetCar",
        method:"post",
    })
}
///需要token
export const GetCarList=(page:number,limit:number,Criteria:string,cityName:string)=>{
    return newAxios({
        url:"/Car/GetCar1",
        method:"post",
        data:Qs.stringify({page:page,limit:limit,searchCriteria:Criteria,cName:cityName}),
    })
}
export const AddCar = (InputCar:any)=>{
    return newAxios({
        url:"/Car/Add",
        method:"post",
        data:InputCar,
        headers: {
            'Content-Type': 'application/json',
            'accept':'text/plain'
        }
    })
}
export const UploadFiles=(param:any)=>{
    return newAxios({
        url:"/UploadFile/UploadFile2",
        method:"post",
        data:param,
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    })
}
export const GetPhone=()=>{
    return newAxios({
        url:"/UserLogin/AnalysisToken",
        method:"post",
    })
}
export const GeConOrder=(page:number)=>{
    return newAxios({
        url:"/Car/GetConOrder",
        method:"post",
        data:Qs.stringify({page:page})
    })
}
export const GetCarInfo=(Id:any)=>{
    return newAxios({
        url:"/Car/GetCarInfo",
        method:"post",
        data:Id
    })
}
export const UpdateState=(data:any)=>{
    return newAxios({
        url:"/Car/UpState",
        method:"post",
        data:data
    })
}

export const UpPassword=(PassWord:any)=>{
    return newAxios({
        url:"UserLogin/ChangePS",
        method:"post",
        data:PassWord
    })
}
export const Test=(id:any)=>{
    return newAxios({
        url:"UserLogin/Test",
        method:"post",
        data:id
    })
}

export const GetSales=(id:any)=>{
    return newAxios({
        url:"UserLogin/GetSalesTicket",
        method:"post",
        data:id
    })
}
export const GetUserInfo=()=>{
    return newAxios({
        url:"UserLogin/GetUserInfo",
        method:"post",
    })
}

export const UpdateUserInfo=(data:any)=>{
    return newAxios({
        url:"UserLogin/UpdateUserInfo",
        method:"post",
        data:data
    })
}
export const GetUserInfo1=(page:number,str:string)=>{
    return newAxios({
        url:"UserLogin/GetUserInfoList",
        method:"post",
        data:Qs.stringify({page:page,searchName:str})
    })
}
export const ResetPasswords=(id:number)=>{
    return newAxios({
        url:"UserLogin/ResetPasswords",
        method:"post",
        data:Qs.stringify({userId:id})
    })
}
export const NegationLock=(id:number)=>{
    return newAxios({
        url:"UserLogin/NegationLock",
        method:"post",
        data:Qs.stringify({userId:id})
    })
}
export const AddUser1=(phone:string,name:string,role:number)=>{
    return newAxios({
        url:"UserLogin/AddUser",
        method:"post",
        data:Qs.stringify({phone:phone,name:name,roleId:role})
    })
}
export const AddUser=(f:any)=>{
    return newAxios({
        url:"UserLogin/AddUser",
        method:"post",
        data:Qs.stringify(f)
    })
}
export const GetNamePhoneByCarId=(Id:number)=>{
    return newAxios({
        url:"UserLogin/GetNamePhoneByCarId",
        method:"post",
        data:Qs.stringify({CarId:Id})
    })
}
export const AddSalesTicket=(carId:number)=>{
    return newAxios({
        url:"UserLogin/AddSalesTicket",
        method:"post",
        data:Qs.stringify({CarId:carId})
    })
}
export const GetSalesList=(page:number)=>{
    return newAxios({
        url:"UserLogin/GetSalesList",
        method:"post",
        data:Qs.stringify({page:page})
    })
}
export const GetSalesTicketBySaleId=(Id:number)=>{
    return newAxios({
        url:"UserLogin/GetSalesTicketBySaleId",
        method:"post",
        data:Qs.stringify({SaleId:Id})
    })
}
export const GetCarListByUserId=(pages:number)=>{
    return newAxios({
        url:"Car/GetCarListByUserId",
        method:"post",
        data:Qs.stringify({page:pages})
    })
}
export const SoldOutById=(id:number)=>{
    return newAxios({
        url:"Car/SoldOutById",
        method:"post",
        data:Qs.stringify({id:id})
    })
}
export const GetSellOrdersList=(pages:number,tf:boolean)=>{
    return newAxios({
        url:"UserLogin/GetSellOrdersList",
        method:"post",
        data:Qs.stringify({page:pages,tf:tf})
    })
}
export const GetSearchCar=(pages:number,limits:number,data:any)=>{
    return newAxios({
        url:"Car/GetSearchCar",
        method:"post",
        data:Qs.stringify({page:pages,limit:limits,searchKey:data})
    })
}
export const GetMenu=()=>{
    return newAxios({
        url:"UserLogin/GetMenu",
        method:"post",
        
    })
}
export const UpSalesAndCarState=(dt:any,bt:number,tf:boolean)=>{
    return newAxios({
        url:"Car/UpSalesAndCarState",
        method:"post",
        data:Qs.stringify({upSales:dt,button:bt,secondLevel:tf})
        
    })
}
//获取验证码
export const GetCode=(phone:string)=>{
    return newAxios({
        url:"Sms/SendSms",
        method:"post",
        data:Qs.stringify({PhoneNum:phone})
    })
}
export const LoginByCode=(phone:string,code:string)=>{
    return newAxios({
        url:"Sms/VerifyCode",
        method:"post",
        data:Qs.stringify({phoneNumber:phone,code:code})
    })
}
