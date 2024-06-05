import PlanModel from "@/models/PlanModel";
import CallResult from "./CallResult"
import constants from "./constants"
import { buildUrl } from 'build-url-ts';
import { useLoginStore } from "@/stores/login";

export async function GetAll(): Promise<CallResult<Array<PlanModel>>> {
    try {
        const loginStore = useLoginStore()

        var url = buildUrl(constants.BASE_URL, {
            path: '/plans/getall',
        });
        if (url == undefined) {
            return new CallResult<Array<PlanModel>>(false, "Could not buid api request");
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
        data.forEach((plan: PlanModel) =>
            {
                let charCopy = JSON.parse(JSON.stringify(plan.characteristics)) as any;
                plan.characteristics = new Map<string, string>();
                if(charCopy == null)
                    return;
                Object.keys(charCopy).forEach((key: string) => {
                    plan.characteristics.set(key, charCopy[key]);
                });
            });

        return new CallResult<Array<PlanModel>>(true, "", data);
    } catch (error: any) {
        // Handle any errors that occur during the API call
        console.error(error)
        //return new CallResult<Array<PlanModel>>(true, "", [new PlanModel("1"), new PlanModel("2")]);
        return new CallResult<Array<PlanModel>>(false, (error as Error).message);
    }
}

export async function GetCurrent(): Promise<CallResult<PlanModel>> {
    try {
        const loginStore = useLoginStore();

        var url = buildUrl(constants.BASE_URL, {
            path: '/plans/getcurrent',
        });
        if (url == undefined) {
            return new CallResult<PlanModel>(false, "Could not buid api request");
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

        let charCopy = JSON.parse(JSON.stringify(data.characteristics)) as any;
        data.characteristics = new Map<string, string>();
        Object.keys(charCopy).forEach((key: string) => {
            data.characteristics.set(key, charCopy[key]);
        });

        return new CallResult<PlanModel>(true, "", data);
    } catch (error: any) {
        // Handle any errors that occur during the API call
        console.error(error)
        //return new CallResult<Array<PlanModel>>(true, "", [new PlanModel("1"), new PlanModel("2")]);
        return new CallResult<PlanModel>(false, (error as Error).message);
    }
}