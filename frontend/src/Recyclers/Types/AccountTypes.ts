export type ProfileResponse = {

}

export interface IAccountService {
    getProfile(): Promise<ProfileResponse>
}