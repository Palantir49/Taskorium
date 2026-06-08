export default function WorkspaceSkeleton() {
    return (
        <div className="relative mx-auto w-full max-w-sm rounded-xl border border-gray-200 p-6 space-y-4 animate-pulse">
            <div className="h-5 w-2/3 rounded bg-gray-200"/>
            <div className="h-3 w-1/3 rounded bg-gray-100"/>
            <div className="flex gap-2 mt-4">
                {[...Array(3)].map((_, i) => (
                    <div key={i} className="h-8 w-8 rounded-full bg-gray-200"/>
                ))}
            </div>
        </div>
    );
}

