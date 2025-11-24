import { IconTrendingDown, IconTrendingUp } from "@tabler/icons-react"

import { Badge } from "@/components/ui/badge"
import {
  Card,
  CardAction,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

const currencyFormatter = new Intl.NumberFormat("vi-VN", {
  style: "currency",
  currency: "VND",
  maximumFractionDigits: 0,
})

type AdminStats = {
  revenue?: number
  tickets?: number
  movies?: number
  shows?: number
}

type AdminSectionCardsProps = {
  stats?: AdminStats
  isLoading?: boolean
}

const formatNumber = (value?: number, isCurrency = false) => {
  if (value === undefined) return "—"
  return isCurrency
    ? currencyFormatter.format(value)
    : new Intl.NumberFormat("vi-VN").format(value)
}

export function AdminSectionCards({
  stats,
  isLoading,
}: AdminSectionCardsProps) {
  const cards = [
    {
      label: "Tổng Doanh Thu",
      value: formatNumber(stats?.revenue, true),
      helper: "Thống kê từ bảng Payments",
      trend: "+15.2%",
      icon: <IconTrendingUp />,
    },
    {
      label: "Tổng Vé Đã Bán",
      value: formatNumber(stats?.tickets),
      helper: "Số lượng booking hợp lệ",
      trend: "+8.5%",
      icon: <IconTrendingUp />,
    },
    {
      label: "Số Phim",
      value: formatNumber(stats?.movies),
      helper: "Bao gồm tất cả trạng thái phim",
      trend: "+3 phim mới",
      icon: <IconTrendingUp />,
    },
    {
      label: "Số Suất Chiếu",
      value: formatNumber(stats?.shows),
      helper: "Đếm từ bảng Shows",
      trend: "-2 suất lỗi",
      icon: <IconTrendingDown />,
    },
  ]

  return (
    <div className="*:data-[slot=card]:from-primary/5 *:data-[slot=card]:to-card dark:*:data-[slot=card]:bg-card grid grid-cols-1 gap-4 px-4 *:data-[slot=card]:bg-gradient-to-t *:data-[slot=card]:shadow-xs lg:px-6 @xl/main:grid-cols-2 @5xl/main:grid-cols-4">
      {cards.map((card) => (
        <Card key={card.label} className="@container/card">
          <CardHeader>
            <CardDescription>{card.label}</CardDescription>
            <CardTitle className="text-2xl font-semibold tabular-nums @[250px]/card:text-3xl">
              {isLoading ? "Đang tải..." : card.value}
            </CardTitle>
            <CardAction>
              <Badge variant="outline">
                {card.icon}
                {card.trend}
              </Badge>
            </CardAction>
          </CardHeader>
          <CardFooter className="flex-col items-start gap-1.5 text-sm">
            <div className="line-clamp-1 flex gap-2 font-medium">
              {card.helper}
            </div>
            <div className="text-muted-foreground">
              Dữ liệu cập nhật theo thời gian thực
            </div>
          </CardFooter>
        </Card>
      ))}
    </div>
  )
}

