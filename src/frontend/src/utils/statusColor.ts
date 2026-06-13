export const COLOR_STATUS = [
    'gray', 'lightblue', 'yellow', 'palegreen', 'pink',
    'lavander', 'green'
];

export const getStableColorById = (position: number): string => {
    return COLOR_STATUS[position];
};