import {
  PageActions,
  PageHeader,
  PageHeaderDescription,
  PageHeaderHeading,
} from "@/components/page-header"
// import { Button } from "@/components/ui/button"
// import { Link } from "react-router"

const title = "Welcome."
const description =
  ""
export default function IndexPage() {
    return (
        <div className="flex flex-1 flex-col ">
            <PageHeader>
                {/* <Announcement /> */}
                <PageHeaderHeading className="max-w-4xl">{title}</PageHeaderHeading>
                    <PageHeaderDescription>{description}</PageHeaderDescription>
                    <PageActions>
                        {/* <Button asChild size="sm">
                            <Link to="/docs/installation">Get Started</Link>
                        </Button>
                        <Button asChild size="sm" variant="ghost">
                            <Link to="/docs/components">View Components</Link>
                        </Button> */}
                    </PageActions>
            </PageHeader>
        </div>
    )
}