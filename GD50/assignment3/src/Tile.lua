--[[
    GD50
    Match-3 Remake

    -- Tile Class --

    Author: Colton Ogden
    cogden@cs50.harvard.edu

    The individual tiles that make up our game board. Each Tile can have a
    color and a variety, with the varietes adding extra points to the matches.
]]

Tile = Class{}

function Tile:init(x, y, color, variety, shine)
    
    -- board positions
    self.gridX = x
    self.gridY = y

    -- coordinate positions
    self.x = (self.gridX - 1) * 32
    self.y = (self.gridY - 1) * 32

    -- tile appearance/points
    self.color = color
    self.variety = variety

    -- check if random number given is 1 to make a shiny block
    if shine == 1 then
        self.shine = true
    else
        self.shine = false
    end
    
end

function Tile:render(x, y)
    
    -- draw shadow
    love.graphics.setColor(34 / 255, 32 / 255, 52 / 255, 255 / 255)
    love.graphics.draw(gTextures['main'], gFrames['tiles'][self.color][self.variety],
        self.x + x + 2, self.y + y + 2)

    -- draw tile itself
    love.graphics.setColor(255 / 255, 255 / 255, 255 / 255, 255 / 255)
    love.graphics.draw(gTextures['main'], gFrames['tiles'][self.color][self.variety],
        self.x + x, self.y + y)

    -- draw opaque white block over tile to make it "shiney"
    if self.shine == true then
        -- multiply so drawing white rect makes it brighter
        love.graphics.setBlendMode('add')

        love.graphics.setColor(255 / 255, 255 / 255, 255 / 255, 70 / 255)
        love.graphics.rectangle('fill', self.x + x,
            self.y + y, 32, 32, 4)
    
        -- back to alpha
        love.graphics.setBlendMode('alpha')
    end

end