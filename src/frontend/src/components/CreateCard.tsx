import { Card, CardContent } from "./ui/card";

interface CreateCardProps {
  title: string;
  onClick: () => void;
}

export default function CreateCard({ title, onClick }: CreateCardProps) {
  return (
    <Card 
      className="border-dashed border-2 border-gray-300 hover:border-blue-500 hover:bg-blue-50 transition-all cursor-pointer h-full"
      onClick={onClick}
    >
      <CardContent className="flex flex-col items-center justify-center py-8 px-4">
        <div className="text-4xl font-light text-gray-400 hover:text-blue-500 mb-2">
          +
        </div>
        <p className="text-sm text-gray-500 text-center">{title}</p>
      </CardContent>
    </Card>
  );
}