import PlanModel from "@/models/PlanModel";
import SelectedPlanModel from "@/models/SelectedPlanModel";

import CallResult from "./CallResult"
import constants, { mapReplacer } from "./constants"
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
        data.forEach((plan: PlanModel) => {
            let charCopy = JSON.parse(JSON.stringify(plan.characteristics)) as any;
            plan.characteristics = new Map<string, string>();
            if (charCopy == null)
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

export async function SelectNewPlan(id: number): Promise<CallResult<SelectedPlanModel>> {
    try {
        const loginStore = useLoginStore()

        var url = buildUrl(constants.BASE_URL, {
            path: '/plans/selectnewplan',
            queryParams: {
                planId: id
            }
        });
        if (url == undefined) {
            return new CallResult<SelectedPlanModel>(false, "Could not buid api request");
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
        if (charCopy != null)
            Object.keys(charCopy).forEach((key: string) => {
                data.characteristics.set(key, charCopy[key]);
            });

        let charOpts = JSON.parse(JSON.stringify(data.characteristicListValues)) as any;
        data.characteristicListValues = new Map<string, string>();
        if (charOpts != null)
            Object.keys(charOpts).forEach((key: string) => {
                data.characteristicListValues.set(key, charOpts[key]);
            });

        return new CallResult<SelectedPlanModel>(true, "", data);
    } catch (error: any) {
        // Handle any errors that occur during the API call
        console.error(error)
        //return new CallResult<Array<PlanModel>>(true, "", [new PlanModel("1"), new PlanModel("2")]);
        return new CallResult<SelectedPlanModel>(false, (error as Error).message);
    }
}

export async function ChangePlan( id: number, chars: Map<string, string>): Promise<CallResult<boolean>> {
    try {
        const loginStore = useLoginStore()
        const params = JSON.stringify({planId: id, chars: chars }, mapReplacer);
        var url = buildUrl(constants.BASE_URL, {
            path: '/plans/ChangePlan',
        });
        if (url == undefined) {
            return new CallResult<boolean>(false, "Could not buid api request");
        }
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + loginStore.data.token
            },
            body: params
        })

        const data = await response.json();
        if (data == undefined) {
            throw new Error("No token in response");
        }
        return new CallResult<boolean>(true, "", data);
    } catch (error: any) {
        // Handle any errors that occur during the API call
        console.error(error)
        //return new CallResult<Array<PlanModel>>(true, "", [new PlanModel("1"), new PlanModel("2")]);
        return new CallResult<boolean>(false, (error as Error).message);
    }
}