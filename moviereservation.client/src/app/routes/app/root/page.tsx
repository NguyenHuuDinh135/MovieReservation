import {
  PageActions,
  PageHeader,
  PageHeaderDescription,
  PageHeaderHeading,
} from "@/components/page-header"
import { Button } from "@/components/ui/button"
import { Link } from "react-router"
// import { Button } from "@/components/ui/button"
// import { Link } from "react-router"

const title = "Gọi Tên Em Đi"
const description =
  "Hard-woking people can't compete against with nepobabies"
export default function IndexPage() {
    return (
        <div className="flex flex-1 flex-col ">
            <PageHeader>
                {/* <Announcement /> */}
                <PageHeaderHeading className="max-w-4xl">{title}</PageHeaderHeading>
                    <PageHeaderDescription>{description}</PageHeaderDescription>
                    <PageActions>
                        <Button asChild size="sm">
                            <Link to="">Get Started</Link>
                        </Button>
                        <Button asChild size="sm" variant="ghost">
                            <Link to="https://github.com/NguyenHuuDinh135/MovieReservation">View Githubs</Link>
                        </Button>
                    </PageActions>
            </PageHeader>
        </div>
    )
}