interface LogInData {
    phoneNumber: string
    password: string
}

interface SignUpData {
    login: LogInData,
    name: string,
    surname: string
}

interface UserData {
    token: string
}

import CallResult from "./CallResult"
import constants from "./constants"
import { buildUrl } from 'build-url-ts';

export async function LogIn(number: string, password: string): Promise<CallResult<UserData>> {
    try {
        var url = buildUrl(constants.BASE_URL, {
            path: '/Auth/login',
            // queryParams: {
            //     phoneNumber: number,
            //     password: password
            // }
        });
        if (url == undefined) {
            return new CallResult<UserData>(false, "Could not buid api request");
        }
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                phoneNumber: number,
                password: password
            } as LogInData)
        })

        const data = await response.text();
        if (data == undefined) {
            throw new Error("No token in response");
        }
        return new CallResult<UserData>(true, "", {
            token: data
        } as UserData);
    } catch (error: any) {
        // Handle any errors that occur during the API call
        console.error(error)
        return new CallResult<UserData>(false, (error as Error).message);
    }
}


export async function SignUp(number: string, password: string, name: string, surname: string): Promise<CallResult<UserData>> {
    try {
        var url = buildUrl(constants.BASE_URL, {
            path: '/Auth/signup',
            // queryParams: {
            //     phoneNumber: number,
            //     password: password
            // }
        });
        if (url == undefined) {
            return new CallResult<UserData>(false, "Could not buid api request");
        }
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                login: {
                    phoneNumber: number,
                    password: password
                } as LogInData,
                name: name,
                surname: surname
            } as SignUpData)
        })

        const data = await response.text();
        if (data == undefined) {
            throw new Error("No token in response");
        }
        return new CallResult<UserData>(true, "", {
            token: data
        } as UserData);
    } catch (error: any) {
        // Handle any errors that occur during the API call
        console.error(error)
        return new CallResult<UserData>(false, (error as Error).message);
    }
}