import CallResult from "./CallResult"
import constants from "./constants"
import { buildUrl } from 'build-url-ts';
import { useLoginStore } from "@/stores/login";
import type UserStats from "@/models/UserModel";

export async function GetStats(): Promise<CallResult<UserStats>> {
    try {
        const loginStore = useLoginStore()

        var url = buildUrl(constants.BASE_URL, {
            path: '/users/getstats',
        });
        if (url == undefined) {
            return new CallResult<UserStats>(false, "Could not buid api request");
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
        return new CallResult<UserStats>(true, "", data);
    } catch (error: any) {
        // Handle any errors that occur during the API call
        console.error(error)
        return new CallResult<UserStats>(false, (error as Error).message);
    }
}
