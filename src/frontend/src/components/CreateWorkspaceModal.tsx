import {useState} from "react";
import {Dialog, DialogClose, DialogContent, DialogHeader, DialogTitle} from "./ui/dialog";
import {Input} from "./ui/input";
import {Button} from "./ui/button";
import {Label} from "./ui/label";
import {createWorkspace} from "../api/workSpaceService";
import {CreateWorkspaceRequest} from "../types/workspace";

interface CreateWorkspaceModalProps {
    isOpen: boolean;
    onOpenChange: (open: boolean) => void;
    onSuccess?: (workspace: any) => void;
}

export default function CreateWorkspaceModal({isOpen, onOpenChange, onSuccess}: CreateWorkspaceModalProps) {
    const [workspaceName, setWorkspaceName] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!workspaceName.trim()) {
            setError("Введите название рабочей области");
            return;
        }

        setIsLoading(true);
        setError(null);

        try {
            const requestData: CreateWorkspaceRequest = {
                name: workspaceName.trim(),
            };

            const newWorkspace = await createWorkspace(requestData);

            setWorkspaceName("");
            onOpenChange(false);

            if (onSuccess) {
                onSuccess(newWorkspace);
            }
        } catch (err) {
            setError("Ошибка при создании рабочей области");
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <Dialog open={isOpen} onOpenChange={onOpenChange}>
            <DialogContent className="sm:max-w-md bg-white text-black border-gray-700">
                <DialogHeader>
                    <DialogTitle>Создать рабочую область</DialogTitle>
                </DialogHeader>
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div className="space-y-2">
                        <Label htmlFor="workspaceName">Название</Label>
                        <Input
                            id="workspaceName"
                            value={workspaceName}
                            onChange={(e) => setWorkspaceName(e.target.value)}
                            placeholder="Введите название рабочей области"
                            disabled={isLoading}
                        />
                    </div>

                    {error && (
                        <div className="text-sm text-red-600 bg-red-50 px-3 py-2 rounded-md">
                            {error}
                        </div>
                    )}

                    <div className="flex justify-end space-x-2 pt-4">
                        <DialogClose asChild>
                            <Button type="button" variant="secondary" disabled={isLoading}>
                                Отмена
                            </Button>
                        </DialogClose>
                        <Button type="submit" disabled={isLoading || !workspaceName.trim()}>
                            {isLoading ? "Создание..." : "Создать"}
                        </Button>
                    </div>
                </form>
            </DialogContent>
        </Dialog>
    );
}