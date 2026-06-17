export function getDateInputValue(date?: string | null): string {
    if (!date) return '';

    return date.split('T')[0];
}

export function formatDateOnlyRu(date?: string | null): string {
    const dateOnly = getDateInputValue(date);
    if (!dateOnly) return '';

    const [year, month, day] = dateOnly.split('-').map(Number);
    if (!year || !month || !day) return dateOnly;

    return new Date(year, month - 1, day).toLocaleDateString('ru-RU', {
        day: 'numeric',
        month: 'long',
        year: 'numeric',
    });
}