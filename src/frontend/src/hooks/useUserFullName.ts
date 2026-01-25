import {useAuth} from 'react-oidc-context';

export const useUserFullName = () => {
    const auth = useAuth();

    if (!auth.user?.profile) return 'Пользователь';

    const {profile} = auth.user;
    const firstName = profile.given_name || profile.name || '';
    const lastName = profile.family_name || '';

    if (firstName && lastName) {
        return `${firstName} ${lastName}`;
    }
    return profile.name || profile.preferred_username || 'Пользователь';
};