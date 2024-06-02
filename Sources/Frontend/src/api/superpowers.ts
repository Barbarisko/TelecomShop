import SuperpowerModel from "@/models/SuperpowerModel";
import CallResult from "./CallResult"
import constants from "./constants"
import { buildUrl } from 'build-url-ts';
import { useLoginStore } from "@/stores/login";

export async function GetAll(): Promise<CallResult<Array<SuperpowerModel>>> {
    try {
        const loginStore = useLoginStore()

        var url = buildUrl(constants.BASE_URL, {
            path: '/superpowers/getall',
        });
        if (url == undefined) {
            return new CallResult<Array<SuperpowerModel>>(false, "Could not buid api request");
        }
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + loginStore.data.token
            }
        })

        const data = await response.json();
        if (data == undefined) {
            throw new Error("No token in response");
        }
        return new CallResult<Array<SuperpowerModel>>(true, "", data);
    } catch (error: any) {
        // Handle any errors that occur during the API call
        console.error(error)
        return new CallResult<Array<SuperpowerModel>>(false, (error as Error).message);
    }
}


export async function GetCurrent(): Promise<CallResult<Array<SuperpowerModel>>> {
    try {
        const loginStore = useLoginStore()

        var url = buildUrl(constants.BASE_URL, {
            path: '/superpowers/getcurrent'
        });
        if (url == undefined) {
            return new CallResult<Array<SuperpowerModel>>(false, "Could not buid api request");
        }
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + loginStore.data.token
            }
        })

        const data = await response.json();
        if (data == undefined) {
            throw new Error("No token in response");
        }
        return new CallResult<Array<SuperpowerModel>>(true, "", data);
    } catch (error: any) {
        // Handle any errors that occur during the API call
        console.error(error)
        return new CallResult<Array<SuperpowerModel>>(false, (error as Error).message);
    }
}
